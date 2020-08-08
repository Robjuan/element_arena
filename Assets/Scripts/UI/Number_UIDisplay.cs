using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Number_UIDisplay : MonoBehaviour
{
    [Tooltip("(Case Sensitive) Match to ModiferType or OutputType")]
    public string m_thisDisplayTarget;

    private WeaponModifier.ModifierType? modTarget = null;

    TextMeshProUGUI m_SelfTMP;

    void Start()
    {
        m_SelfTMP = gameObject.GetComponent<TextMeshProUGUI>();
        
        // remove this when gauges are working
        if (Enum.TryParse<WeaponModifier.ModifierType>(m_thisDisplayTarget, out WeaponModifier.ModifierType modTarg))
        {
            if(modTarg != WeaponModifier.ModifierType.None)
            {
                GameEvents.current.onModChange += UpdateNumberDisplay;
                modTarget = modTarg;
            }

        } else if (Enum.TryParse<WeaponController.OutputType>(m_thisDisplayTarget, out WeaponController.OutputType outTarget))
        {
            switch (outTarget)
            {
                case WeaponController.OutputType.Mass:
                    GameEvents.current.onMassChange += UpdateNumberDisplay;
                    break;
                case WeaponController.OutputType.InitialVelocity:
                    GameEvents.current.onInitVelocChange += UpdateNumberDisplay;
                    break;
            }
            
        } else if (m_thisDisplayTarget == "Health")
        {
            GameEvents.current.onHealthChange += UpdateNumberDisplay;
        }
    }

    private void UpdateNumberDisplay(float newVal)
    {
        m_SelfTMP.SetText(newVal.ToString());
    }

    private void UpdateNumberDisplay(Dictionary<WeaponModifier.ModifierType, float> modDict)
    {
        if(modTarget != null)
        {
            m_SelfTMP.SetText(modDict[(WeaponModifier.ModifierType)modTarget].ToString());
        }
    }
}
