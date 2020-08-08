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

    private void Start()
    {
        // hit update once each on start to set up display vals
        foreach (WeaponModifier mod in GetWeaponModifiers())
        {
            UpdateModifier(0, mod);
        }
    }

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

    public int GetCurrentUsedPoints()
    {
        var total = 0;
        foreach(WeaponModifier mod in GetWeaponModifiers())
        {
            total += (int)mod.modifierValue;
        }
        return total;
    }

    public Dictionary<WeaponModifier.ModifierType, float> GetCurrentModDict()
    {
        Dictionary<WeaponModifier.ModifierType, float>  modDict = new Dictionary<WeaponModifier.ModifierType, float> { };
        foreach (WeaponModifier mod in GetWeaponModifiers())
        {
            modDict.Add(mod.modifierType, mod.modifierValue);
        }
        return modDict;
    }

    public void ApplyModifiers(ProjectileBase projectile)
    {
        foreach (var mod in GetWeaponModifiers())
        {
            mod.ApplyModifier(projectile);
        }
        
    }

    public bool CanUpdateModifier(int delta, WeaponModifier mod)
    {
        Debug.Log("wmm mpa: " + m_PointsAvailable);
        var goalValue = mod.modifierValue + delta;
        if (goalValue < 1 || goalValue > m_PointsAvailable)
        {
            //Debug.Log("goal value outside range");
            return false;            
        }

        var mods = GetWeaponModifiers();

        var current_sum = 0;
        for (int i = 0; i < mods.Count; i++)
        {
            current_sum += mods[i].modifierValue;
        }

        if ((current_sum+delta) > m_PointsAvailable)  
        {
            //Debug.Log("not enough points avail: "+current_sum);
            return false;
        }

        return true;
    }

    public void UpdateModifier(int delta, WeaponModifier mod)
    {
        if(CanUpdateModifier(delta, mod))
        {
            mod.modifierValue += delta;
            GameEvents.current.ModChange(GetCurrentModDict());
        }
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

    public int GetModifyScrollInput()
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

    public void CheckAndHandleModifyInputs()
    { 
        var modInput = GetModifyInput();
        if (modInput > -1)
        {
            var modDirection = GetModifyScrollInput();
            if (modDirection != 0)
            {
                var mods = GetWeaponModifiers();
                UpdateModifier(modDirection, mods[modInput]);
            }
        }
    }

}