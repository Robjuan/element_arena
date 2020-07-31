using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Number_UIDisplay : MonoBehaviour
{
    [Tooltip("(Case Sensitive) Match to ModiferType or OutputType")]
    public string m_thisDisplayTarget;

    TextMeshProUGUI m_SelfTMP;

    void Start()
    {
        m_SelfTMP = gameObject.GetComponent<TextMeshProUGUI>();
        
        // remove this when gauges are working
        if (Enum.TryParse<WeaponModifier.ModifierType>(m_thisDisplayTarget, out WeaponModifier.ModifierType modTarget))
        {
            switch (modTarget)
            {
                case WeaponModifier.ModifierType.Force:
                    GameEvents.current.onForceModChange += UpdateNumberDisplay;
                    break;
                case WeaponModifier.ModifierType.Size:
                    GameEvents.current.onSizeModChange += UpdateNumberDisplay;
                    break;
                case WeaponModifier.ModifierType.Temperature:
                    GameEvents.current.onTempModChange += UpdateNumberDisplay;
                    break;
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
}
