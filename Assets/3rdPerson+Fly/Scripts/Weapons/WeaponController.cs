using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public Transform projectileSpawnPoint  {get; set;}

    [Tooltip("Weapon Modifier Manager, looks after changing modifiers")]
    public WeaponModifierManager weaponModifierManager;

    [Tooltip("The projectile prefab")]
    public ProjectileBase projectilePrefab;

    [Tooltip("Delay between fires")]
    public float shotDelay;

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
    }

    private void Update()
    {
        checkOutputs();
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
                    var testProj = CreateModifiedProjectile(weaponModifierManager, projectilePrefab, new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,0,0)));
                    newest_mass = testProj.GetMassFromSize();
                    Destroy(testProj.gameObject);
                    last_sizemod = weaponModifierManager.GetWeaponModifier(WeaponModifier.ModifierType.Size).modifierValue;
                }

                if (ot == OutputType.Mass)// && lastValues[ot] != newest_mass)
                {
                    GameEvents.current.OutputChange(newest_mass, OutputType.Mass);
                    lastValues[ot] = newest_mass;
                    //continue;
                }            
                else if (ot == OutputType.InitialVelocity)// && lastValues[ot] != newest_mass)
                {
                    // consider tracking forcemod changes like sizemod
                    var forceMod = weaponModifierManager.GetWeaponModifier(WeaponModifier.ModifierType.Force);
                    var shootforce = forceMod.modifierValue * forceMod.modifierScale;
                    float v = (shootforce/newest_mass)*Time.fixedDeltaTime;
                    GameEvents.current.OutputChange((v * 100), OutputType.InitialVelocity); // scale it up for display
                    lastValues[ot] = v;
                    //continue; // redundant as last option
                }
                
            }
            else if (ot == OutputType.Density)
            {
                GameEvents.current.OutputChange(projectilePrefab.density,OutputType.Density);
            }
        }
    }
    
    public ProjectileBase CreateModifiedProjectile(WeaponModifierManager modman, ProjectileBase prefab, Vector3 position, Quaternion rot)
    {
            ProjectileBase newProjectile = Instantiate(prefab, position, rot);
            modman.ApplyModifiers(newProjectile);
            return newProjectile;
    }

    public void HandleShoot()
    {
        if ((m_LastTimeShot + shotDelay) < Time.time)
        {
            Vector3 shotDirection = GetShotDirection(); 
            var newproj = CreateModifiedProjectile(weaponModifierManager, projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(shotDirection));

            newproj.Shoot(shotDirection);
            m_LastTimeShot = Time.time;
        }
    }

    private Vector3 GetShotDirection()
    {
        // specifically aim towards the crosshair
        return Camera.main.transform.forward;
    }
}