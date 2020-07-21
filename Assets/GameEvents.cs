using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    // Start is called before the first frame update
    void Awake()
    {
       current = this;
    }

    public event Action<float, WeaponModifier.ModifierType> onModifierChange;
    public void ModifierChange(float newVal, WeaponModifier.ModifierType modType)
    {
        if (onModifierChange != null)
        {
            onModifierChange(newVal, modType);
        }
    }    

    public event Action<float, WeaponController.OutputType> onOutputChange;
    public void OutputChange(float newVal, WeaponController.OutputType outType)
    {
        if (onOutputChange != null)
        {
            onOutputChange(newVal, outType);
        }
    }    
}
