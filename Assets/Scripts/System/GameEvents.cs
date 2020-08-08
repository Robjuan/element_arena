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

    public event Action<WeaponController> onWeaponChange;
    public void WeaponChange(WeaponController newActiveWeapon)
    {
        if (onWeaponChange != null)
        {
            onWeaponChange(newActiveWeapon);
        }
    }

    #region modifier change events

    public event Action<Dictionary<WeaponModifier.ModifierType, float>> onModChange;
    public void ModChange(Dictionary<WeaponModifier.ModifierType, float> modDict)
    {
        if (onModChange != null)
        {
            onModChange(modDict);
        }
    }

    public event Action<float> onSizeModChange;
    public void SizeModChange(float newVal)
    {
        if (onSizeModChange != null)
        {
            onSizeModChange(newVal);
        }
    }

    public event Action<float> onForceModChange;
    public void ForceModChange(float newVal)
    {
        if (onForceModChange != null)
        {
            onForceModChange(newVal);
        }
    }

    public event Action<float> onTempModChange;
    public void TempModChange(float newVal)
    {
        if (onTempModChange != null)
        {
            onTempModChange(newVal);
        }
    }

    #endregion

    #region output change events

    public event Action<float> onManaChange;
    public void ManaChange(float newVal)
    {
        if (onManaChange != null)
        {
            onManaChange(newVal);
        }
    }

    public event Action<float> onManaCostChange;
    public void ManaCostChange(float newVal)
    {
        if (onManaCostChange != null)
        {
            onManaCostChange(newVal);
        }
    }

    public event Action<float> onInitVelocChange;
    public void InitVelocChange(float newVal)
    {
        if (onInitVelocChange != null)
        {
            onInitVelocChange(newVal);
        }
    }

    public event Action<float> onMassChange;
    public void MassChange(float newVal)
    {
        if (onMassChange != null)
        {
            onMassChange(newVal);
        }
    }

    #endregion

    #region UI intra-events

    public event Action onGaugeInnerUpdate;
    public void GaugeInnerUpdate()
    {
        if (onGaugeInnerUpdate != null)
        {
            onGaugeInnerUpdate();
        }
    }

    #endregion

    public event Action<float> onHealthChange;
    public void HealthChange(float newVal)
    {
        if (onHealthChange != null)
        {
            onHealthChange(newVal);
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
