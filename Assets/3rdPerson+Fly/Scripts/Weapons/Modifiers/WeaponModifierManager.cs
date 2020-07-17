using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponModifierManager : MonoBehaviour
{
    [Tooltip("Modifiers Applied to this Weapon")]
    public List<WeaponModifier> l_modifiers;
   
    [Tooltip("Size scale")]
    public float sizeScale;
    [Tooltip("Speed scale")]
    public float speedScale;
    [Tooltip("Temperature scale")]
    public float tempScale;
    
    [Tooltip("Maximum points available across all modifiers")]
    public int m_PointsAvailable;

    private string modscroll = "Mouse ScrollWheel";

    // called on start by WeaponModifier
    public void RegisterModifier(WeaponModifier modifier)
    {
        l_modifiers.Add(modifier);
    }

    public List<WeaponModifier> GetWeaponModifiers ()
    {
        return l_modifiers;
    }

    public void ApplyModifiers(ProjectileBase projectile)
    {
        foreach (var mod in l_modifiers)
        {
            if (!mod.modifierActive)
            {
                continue;
            }
            
            if (mod.modifierName.ToLower() == "size")
            {
                float scale = mod.modifierValue * sizeScale;
                projectile.transform.localScale = new Vector3(scale,scale,scale);

                continue;
            }

            if (mod.modifierName.ToLower() == "speed")
            {
                float scale = mod.modifierValue * speedScale;
                projectile.shootForce = scale;

                continue;
            }
            if (mod.modifierName.ToLower() == "temperature")
            {
                float scale = mod.modifierValue * tempScale;
                projectile.temperature = scale;
                continue;
            }
            
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