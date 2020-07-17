using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // currently being set by PlayerWeaponsManager on AddWeapon
    // as the spawnpoint is currently controlled by the player object
    //[Tooltip("Location for projectile to spawn at")]
    public Transform projectileSpawnPoint  {get; set;}

    [Tooltip("Weapon Modifier Manager, looks after changing modifiers")]
    public WeaponModifierManager weaponModifierManager;

    [Tooltip("The projectile prefab")]
    public ProjectileBase projectilePrefab;

    [Tooltip("Delay between fires")]
    public float shotDelay;
 
    float m_LastTimeShot = Mathf.NegativeInfinity;

    public void HandleShoot()
    {
        if ((m_LastTimeShot + shotDelay) < Time.time)
        {
            Vector3 shotDirection = GetShotDirection(); 
            ProjectileBase newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(shotDirection));

            if (weaponModifierManager != null)
            {
                weaponModifierManager.ApplyModifiers(newProjectile);
            }

            newProjectile.Shoot(shotDirection);
            m_LastTimeShot = Time.time;
        }
    }

    private Vector3 GetShotDirection()
    {
        // specifically aim towards the crosshair
        return Camera.main.transform.forward;
    }
}