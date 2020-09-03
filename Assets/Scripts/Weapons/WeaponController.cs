using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;

public class WeaponController : MonoBehaviour
{
    // if you fuck with these at all you have to check every reference in inspector
    public enum OutputType 
    {
        None,
        Mass,
        Density,
        InitialVelocity
    }

    // currently being set by PlayerWeaponsManager on AddWeapon
    // as the spawnpoint is currently controlled by the player object
    //Tooltip("Location for projectile to spawn at")]
    public Transform projectileAnchorSpawnPoint  {get; set;}
    private Vector3 projectileSpawnPosition;

    [Tooltip("Weapon Modifier Manager, looks after changing modifiers")]
    public WeaponModifierManager weaponModifierManager;

    [Tooltip("The projectile prefab")]
    public string projectilePrefabTag;
    private ProjectileBase placeholderProjectilePrefab;
    private ProjectileBase placeholderProjectile;

    [Tooltip("Delay between fires")]
    public float shotDelay;

    [Tooltip("scales the cost of casting")]
    public float manaScale;
    private float currentManaCost;
    private float lastManaCost;

    [Tooltip("Timing & Pace of placeholder fade; reset on change")]
    public float placeholderFadeTime;
    public float placeholderFadeDelay;
    private float currentFadeTime = 0f;
    private float lastPlaceholderFadeReset;

    [Tooltip("UI will overwrite alpha")]
    public Color activeUIColor;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public ManaUser playerMana;
    
    [HideInInspector]
    public bool isActiveWeapon = false;

    // last value trackers
    // > outputs
    private Dictionary<OutputType, float> lastValues;
    // > modifiers
    private float last_sizemod = 0.0f;

    float m_LastTimeShot = Mathf.NegativeInfinity;
    private float newest_mass = 0.0f;

    private void Awake()
    {
        // create dict where key = outputType, value = last value
        // these track the last values of the OUTPUTS and thus whether or not to update UI
        // but we often need to know if the modifier has changed also to check whether or not to re-calc
        // TODO: put those modifier trackers in a nice dictionary
        lastValues = Enum.GetValues(typeof(OutputType)).Cast<OutputType>().Where(w => w != OutputType.None).ToDictionary(v => v, v => 0.0f);
        // todo: better?
        player = GameObject.FindWithTag("Player");
        playerMana = player.GetComponent<ManaUser>();

        GameEvents.current.onModChange += UpdateCurrentManacost;
    }

    private void Start()
    { 
        projectileSpawnPosition = projectileAnchorSpawnPoint.position;
    }

    private void Update()
    {
        if (isActiveWeapon)
        {
            checkOutputs();

            // should this be in fixedupdate?
            UpdatePlaceholder();
        }
    }

    public void checkOutputs()
    {
        // use toList here because we will modify the real lastValues while still looping
        foreach(OutputType ot in lastValues.Keys.ToList())
        {
            // things that need a test projectile
            if (ot == OutputType.Mass || ot == OutputType.InitialVelocity)
            {
                // only do a new projectile if size has changed
                if(last_sizemod != weaponModifierManager.GetWeaponModifier(WeaponModifier.ModifierType.Size).modifierValue)
                {
                    if (placeholderProjectile)
                    {
                        newest_mass = placeholderProjectile.GetMassFromSize();
                    } else
                    {
                        var testProj = CreateModifiedProjectile(weaponModifierManager, projectilePrefabTag, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                        newest_mass = testProj.GetMassFromSize();
                        testProj.gameObject.SetActive(false);
                    }
                    last_sizemod = weaponModifierManager.GetWeaponModifier(WeaponModifier.ModifierType.Size).modifierValue;
                }

                if ((ot == OutputType.Mass) && lastValues[ot] != newest_mass)
                {
                    GameEvents.current.MassChange(Mathf.Round(newest_mass));
                    lastValues[ot] = newest_mass;
                    continue;
                }            
                else if ((ot == OutputType.InitialVelocity) && lastValues[ot] != newest_mass)
                {
                    // consider tracking forcemod changes like sizemod
                    var forceMod = weaponModifierManager.GetWeaponModifier(WeaponModifier.ModifierType.Force);
                    var shootforce = forceMod.modifierValue * forceMod.modifierScale;
                    float v = (shootforce/newest_mass)*Time.fixedDeltaTime;
                    GameEvents.current.InitVelocChange(Mathf.Round((v * 100))); // scale it up for display
                    lastValues[ot] = v;
                    continue;
                }
                
            }
            else if (ot == OutputType.Density)
            {
                //Debug.Log(projectilePrefab.density);
            }
        }
    }
    
    public void SetActive()
    {
        if (!isActiveWeapon)
        {
            isActiveWeapon = true;
            GameEvents.current.ModChange(weaponModifierManager.GetCurrentModDict());
            lastPlaceholderFadeReset = Time.time;
            this.gameObject.SetActive(true);
        }
    }

    public void SetInactive()
    {
        if (isActiveWeapon)
        {
            isActiveWeapon = false;
            Destroy(placeholderProjectile.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    public ProjectileBase CreateModifiedProjectile(WeaponModifierManager modman, string prefabtag, Vector3 position, Quaternion rot)
    {
        ProjectileBase newProjectile = ObjectPooler.current.SpawnFromPool(prefabtag, position, rot).GetComponent<ProjectileBase>();
        modman.ApplyModifiers(newProjectile);
        return newProjectile;
    }

    public void UpdateCurrentManacost(Dictionary<WeaponModifier.ModifierType, float> modDict)
    {
        var manacost = 0f;
        foreach (float modVal in modDict.Values)
        {
            manacost += modVal;
        }
        currentManaCost = manacost * manaScale;

        if (currentManaCost != lastManaCost)
        {
            GameEvents.current.ManaCostChange(currentManaCost, playerMana.maxMana);
            lastManaCost = currentManaCost;
            
            if(placeholderProjectile != null)
            {
                // this should potentially be called by modchange, but this func is called by modchange so that's ok
                lastPlaceholderFadeReset = Time.time;
                currentFadeTime = 0f;
                placeholderProjectile.UpdateTransparency(1);
            }
        }
    }

    private void UpdatePlaceholder()
    {
        if (placeholderProjectilePrefab == null)
        {
            placeholderProjectilePrefab = ObjectPooler.current.GetPoolPrefab(projectilePrefabTag).GetComponent<ProjectileBase>();
        }    
        if (!placeholderProjectile && isActiveWeapon)
        {
            placeholderProjectile = Instantiate(placeholderProjectilePrefab, projectileSpawnPosition, Quaternion.LookRotation(GetShotDirection()));
            // placeholderProjectile.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // slowly rotate ?
            placeholderProjectile.isPlaceholder = true;
        }

        weaponModifierManager.ApplyModifiers(placeholderProjectile);
        var projRadius = placeholderProjectile.GetRadius();
        
        projectileSpawnPosition = projectileAnchorSpawnPoint.position + (GetShotDirection() * projRadius * 2) + (transform.up * projRadius / 1.5f);

        placeholderProjectile.transform.position = projectileSpawnPosition;

        // if float delay has passed since last update, start fade
        if (lastPlaceholderFadeReset + placeholderFadeDelay < Time.time)
        {
            currentFadeTime += Time.deltaTime;
            if (currentFadeTime > placeholderFadeTime)
            {
                currentFadeTime = placeholderFadeTime;
            }
            var perc = currentFadeTime / placeholderFadeTime;
            var max = placeholderProjectile.maxTransparencyValue;
            placeholderProjectile.UpdateTransparency(Mathf.Lerp(max, 0, perc));
        }
    }

    private bool CanShoot()
    {
        bool firerate = false;
        bool manacost = false;

        // fire rate
        if ((m_LastTimeShot + shotDelay) < Time.time)
        {
            firerate = true;
        }
        
        // manacost
        if (playerMana.GetCurrentMana() > currentManaCost)
        {
            manacost = true;
        }

        // playerweaponsmanager will call handleshoot on the activeweapon only
        return firerate && manacost && isActiveWeapon;
    }

    public void HandleShoot()
    {
        if (CanShoot())
        {
            // todo: take these maths out so we're not doubling projectiles
            // todo: some raycasting to check the furthest away we can spawn without spawning on the other side of something

            //Vector3 adjustedSpawnPoint = player.transform.position + 
            
            Vector3 shotDirection = GetShotDirection();             
            var newproj = CreateModifiedProjectile(weaponModifierManager, projectilePrefabTag, projectileSpawnPosition, Quaternion.LookRotation(shotDirection));
            newproj.Shoot(shotDirection);
            m_LastTimeShot = Time.time;

            var newMana = playerMana.UpdateCurrentMana(-currentManaCost);
            GameEvents.current.ManaChange(newMana);
        }
    }

    private Vector3 GetShotDirection()
    {
        // specifically aim towards the crosshair
        // todo: give a slight upwards tilt
        return Camera.main.transform.forward;
    }
}