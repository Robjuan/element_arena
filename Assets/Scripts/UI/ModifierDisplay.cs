using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// todo : redo this with an event system??

public class ModifierDisplay : MonoBehaviour
{
    [Tooltip("Target Modifier Type")]
    public WeaponModifier.ModifierType m_TargetModifierType;

    [Tooltip("Target Modifier Type")]
    public WeaponController.OutputType m_TargetOutputType;

    TextMeshProUGUI m_SelfTMP;

    // Start is called before the first frame update
    void Start()
    {
        m_SelfTMP = gameObject.GetComponent<TextMeshProUGUI>();

        GameEvents.current.onModifierChange += UpdateModifierDisplay;
        GameEvents.current.onOutputChange += UpdateOutputDisplay;
    }

    // event listeners
    private void UpdateModifierDisplay(float newVal, WeaponModifier.ModifierType modType)
    {
        if (m_TargetModifierType == modType)
        {
            // inputs should always be ints
            m_SelfTMP.SetText(newVal.ToString());
        }
    }

    private void UpdateOutputDisplay(float newVal, WeaponController.OutputType outType)
    {
        if (m_TargetOutputType == outType)
        {
            m_SelfTMP.SetText(Math.Round(newVal).ToString());
        }
    }
}
