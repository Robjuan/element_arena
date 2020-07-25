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

    public event Action<float, Number_UIDisplay.DisplayTarget> onHealthChange;
    public void HealthChange(float newVal, Number_UIDisplay.DisplayTarget displayTarget)
    {
        if (onHealthChange != null)
        {
            onHealthChange(newVal, displayTarget);
        }
    } 

    public event Action<GameObject> onActorDeath;
    public void ActorDeath(GameObject actor)
    {
        if (onActorDeath != null)
        {
            onActorDeath(actor);
        }
    }

    public event Action<GameObject> onSpawnerEmpty;
    public void SpawnerEmpty(GameObject spawner)
    {
        if (onSpawnerEmpty != null)
        {
            onSpawnerEmpty(spawner);
        }
    }
}
