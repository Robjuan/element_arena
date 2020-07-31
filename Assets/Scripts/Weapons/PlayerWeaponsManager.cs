using UnityEngine;
using System.Collections.Generic;

public class PlayerWeaponsManager : MonoBehaviour
{

    [Tooltip("List of weapon the player will start with")]
    public List<WeaponController> startingWeapons = new List<WeaponController>();
    public Transform weaponParent;

    public Transform projectileSpawnPoint;

    public string shootButton = "Fire1";

    WeaponController[] weaponSlots = new WeaponController[5]; // 5 available weapon slots
    private int activeWeaponIndex;

    private void Start()
    {
        activeWeaponIndex = 0;
        foreach (var weapon in startingWeapons)
        {
            AddWeapon(weapon);
        } 

    }

    // Adds a weapon to our inventory
    public bool AddWeapon(WeaponController weaponPrefab)
    {
        // go through all weapon slots
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            // only add the weapon if the slot is free
            if(weaponSlots[i] == null)
            {
                
                WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParent);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;

                // set the projectile spawn point to the player
                // TODO: if this is on the weapon or player depends on if the weapons are "upgrades" or not, and if the spell comes out of them or player
                weaponInstance.projectileSpawnPoint = this.projectileSpawnPoint;

                // Handle auto-switching to weapon if no weapons currently

                weaponSlots[i] = weaponInstance;
                SetActiveWeaponSlot(i);
                return true;
            }
        }

        return false;
    }

    public void SetActiveWeaponSlot(int slot)
    {
        if (slot <= weaponSlots.Length && slot >= 0)
        {
            activeWeaponIndex = slot;
            GameEvents.current.WeaponChange(GetActiveWeapon());
        }
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        // find the active weapon in our weapon slots based on our active weapon index
        if(index >= 0 &&
            index < weaponSlots.Length)
        {
            return weaponSlots[index];
        }

        // if we didn't find a valid active weapon in our weapon slots, return null
        return null;
    }

    private void Update()
    {
        if(Input.GetButton(shootButton))
        {
            WeaponController current_WeaponController = GetActiveWeapon();
            current_WeaponController.HandleShoot();
        }
    }
    
}    