using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Number_UIDisplay : MonoBehaviour
{
    public Image backgroundColorImage;
    public Color warningColor;
    private Color baseColor;

    [Tooltip("(Case Sensitive) Match to ModiferType or OutputType")]
    public string m_thisDisplayTarget;

    private WeaponModifier.ModifierType? modTarget = null;

    TextMeshProUGUI m_SelfTMP;

    void Start()
    {
        baseColor = backgroundColorImage.color;

        m_SelfTMP = gameObject.GetComponent<TextMeshProUGUI>();
        
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

    // health & outputs use this overload
    private void UpdateNumberDisplay(float newVal)
    {
        m_SelfTMP.SetText(newVal.ToString());
        StartCoroutine("FlashWarningColor");

    }

    private IEnumerator FlashWarningColor()
    {
        // this isn't great with multiple sources of damage, potentially check if warning is currently active and then just sustain it?
        // multiple coroutines can be active simultaneously, might need to set a bool and have it get checked on update?
        // need to keep track of the last time we took damage and check the time since then a la fire rate.s
        // new damage instances then only need to update hte "last damaged" time
        backgroundColorImage.color = warningColor;
        yield return new WaitForSeconds(1f);
        backgroundColorImage.color = baseColor;
    }

    // weaponmods use this overload
    private void UpdateNumberDisplay(Dictionary<WeaponModifier.ModifierType, float> modDict)
    {
        if(modTarget != null)
        {
            m_SelfTMP.SetText(modDict[(WeaponModifier.ModifierType)modTarget].ToString());
        }
    }
}
