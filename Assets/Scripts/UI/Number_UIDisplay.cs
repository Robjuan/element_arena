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
    public float warningDuration;
    private Color baseColor;
    private float lastWarningTime;

    [Tooltip("(Case Sensitive) Match to ModiferType or OutputType")]
    public string m_thisDisplayTarget;

    private WeaponModifier.ModifierType? modTarget = null;

    TextMeshProUGUI m_SelfTMP;

    void Start()
    {
        lastWarningTime = Mathf.NegativeInfinity;
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
        // this should only flash on damage not heal (healthchange event should carry a type or something)
        lastWarningTime = Time.time;

    }

    // weaponmods use this overload
    private void UpdateNumberDisplay(Dictionary<WeaponModifier.ModifierType, float> modDict)
    {
        if(modTarget != null)
        {
            m_SelfTMP.SetText(modDict[(WeaponModifier.ModifierType)modTarget].ToString());
        }
    }

    void Update()
    {
        // this will set the color every frame - potentially not great performance
        if (lastWarningTime + warningDuration > Time.time)
        {
            backgroundColorImage.color = warningColor;
        } else
        {
            backgroundColorImage.color = baseColor;
        }
    }

}
