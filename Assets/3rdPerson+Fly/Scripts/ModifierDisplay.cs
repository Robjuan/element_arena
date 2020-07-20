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


    PlayerWeaponsManager m_WeaponsManager;
    WeaponController m_ActiveWeapon;
    TextMeshProUGUI m_SelfTMP;
    WeaponController m_LastWeapon;
    WeaponModifierManager m_ModifierManager;

    // Start is called before the first frame update
    void Start()
    {
        m_WeaponsManager = FindObjectOfType<PlayerWeaponsManager>();
        m_SelfTMP = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // called on update() by HUDManager
    public void UpdateSelf()
    {
        m_ActiveWeapon = m_WeaponsManager.GetActiveWeapon();
        if (m_TargetOutputType != WeaponController.OutputType.None)
        {
           GetandUpdateOutputs();
        }
        else if (m_TargetModifierType != WeaponModifier.ModifierType.None)
        {
            GetandUpdateModifiers();
        }
    }

    void GetandUpdateModifiers()
    {
        //m_LastWeapon = m_ActiveWeapon;
        //m_ActiveWeapon = m_WeaponsManager.GetActiveWeapon();

        if (m_ActiveWeapon.weaponModifierManager != null){

            m_ModifierManager = m_ActiveWeapon.weaponModifierManager;
            var currentWeaponModifiers = m_ModifierManager.GetWeaponModifiers();

            for (int i = 0; i < currentWeaponModifiers.Count; i++)
            {
                if (currentWeaponModifiers[i].modifierType == m_TargetModifierType)
                {
                    m_SelfTMP.SetText(currentWeaponModifiers[i].modifierValue.ToString());
                    break;
                }
            }    
        }
    }

    void GetandUpdateOutputs()
    {       
        foreach(var output in Enum.GetValues(typeof(WeaponController.OutputType)))
        {
            var outputType = (WeaponController.OutputType)output;
            if (m_TargetOutputType == outputType)
            {
                var outputValue = m_ActiveWeapon.GetOutput(outputType, m_ActiveWeapon.projectilePrefab);
                m_SelfTMP.SetText(Math.Round(outputValue).ToString());
                break;
            }
        }
    }
}
