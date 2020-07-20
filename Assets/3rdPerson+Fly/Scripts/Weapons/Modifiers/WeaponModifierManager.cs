using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponModifierManager : MonoBehaviour
{
    [Tooltip("Modifiers Applied to this Weapon")]
    public List<WeaponModifier> l_modifiers;
      
    [Tooltip("Maximum points available across all modifiers")]
    public int m_PointsAvailable;

    private string modscroll = "Mouse ScrollWheel";

    // called on start by WeaponModifier
    public void RegisterModifier(WeaponModifier modifier)
    {
        l_modifiers.Add(modifier);
    }

    public WeaponModifier GetWeaponModifier(WeaponModifier.ModifierType modType)
    {
        foreach (var mod in GetWeaponModifiers())
        {
            if (mod.modifierType == modType)
            {
                return mod;
            }
        }
        return null;
    }

    public List<WeaponModifier> GetWeaponModifiers()
    {
        return l_modifiers;
    }

    public void ApplyModifiers(ProjectileBase projectile)
    {
        foreach (var mod in GetWeaponModifiers())
        {
            mod.ApplyModifier(projectile);
        }
        
    }

    public bool CanUpdateModifier(float delta, WeaponModifier mod)
    {
        var goalValue = mod.modifierValue + delta;
        if (goalValue < 0 || goalValue > 999)
        {
            Debug.Log("goal value outside range");
            return false;            
        }

        var mods = GetWeaponModifiers();

        var current_sum = 0;
        for (int i = 0; i < mods.Count; i++)
        {
            current_sum += (int)Math.Round(mods[i].modifierValue);
        }

        if ((current_sum+delta) > m_PointsAvailable)  
        {
            Debug.Log("not enough points avail: "+current_sum);
            return false;
        }

        return true;
    }

    public void UpdateModifier(float delta, WeaponModifier mod)
    {
        if(CanUpdateModifier(delta, mod))
        {
            
            mod.modifierValue += delta;
            GameEvents.current.ModifierChange(mod.modifierValue, mod.modifierType);
            // highlight in green
        }
        // highlight in red
    }

    public int GetModifyInput()
    {
        var mods = GetWeaponModifiers();
        for (int i = 0; i < mods.Count; i++)
        {
            if (Input.GetButton(mods[i].modifierButton))
            {
                return i;
            }
        }
        return -1;
    }

    public float GetModifyScrollInput()
    {
        if (Input.GetAxis(modscroll) < 0f)
        {
            return -1;
        }         
        else if (Input.GetAxis(modscroll) > 0f)
        {
            return 1;
        }
        return 0;
    }

    void Update()
    {
        var modInput = GetModifyInput();
        if (modInput > -1)
        {
            var modAmount = GetModifyScrollInput();
            var mods = GetWeaponModifiers();

            UpdateModifier(modAmount, mods[modInput]);
            
        }
    }

}