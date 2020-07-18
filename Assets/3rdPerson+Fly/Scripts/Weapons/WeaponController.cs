using UnityEngine;

public class WeaponController : MonoBehaviour
{
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
    
    private float last_sizemod = 0.0f;
    private float newest_mass = 0.0f;
 
    float m_LastTimeShot = Mathf.NegativeInfinity;

    public float GetOutput(OutputType outputType, ProjectileBase projectile1)
    {
        // things that require a test projectile
        // ie, things that require mass
        if (outputType == OutputType.Mass || outputType == OutputType.InitialVelocity)
        {
            // only do a new projectile if size has changed
            if(last_sizemod != weaponModifierManager.GetWeaponModifier(WeaponModifier.ModifierType.Size).modifierValue)
            {
                var testProj = CreateModifiedProjectile(weaponModifierManager, projectile1, new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,0,0)));
                newest_mass = testProj.GetMassFromSize();
                Destroy(testProj.gameObject);
            }
            last_sizemod = weaponModifierManager.GetWeaponModifier(WeaponModifier.ModifierType.Size).modifierValue;

            if (outputType == OutputType.Mass)
            {
                return newest_mass;
            }            
            else if (outputType == OutputType.InitialVelocity)
            {
                // consider tracking forcemod changes like sizemod
                var forceMod = weaponModifierManager.GetWeaponModifier(WeaponModifier.ModifierType.Force);
                var shootforce = forceMod.modifierValue * forceMod.modifierScale;
                float v = (shootforce/newest_mass)*Time.fixedDeltaTime;
                return v * 100; // scale it up for display
            }
            
        }
        if (outputType == OutputType.Density)
        {
            // prefab density not the instance density
            // will need to update this if density can be modified (unlikely)
            return projectilePrefab.density;
        }

        return -1;
    }

    public ProjectileBase CreateModifiedProjectile(WeaponModifierManager modman, ProjectileBase prefab, Vector3 position, Quaternion rot)
    {
            ProjectileBase newProjectile = Instantiate(projectilePrefab, position, rot);
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