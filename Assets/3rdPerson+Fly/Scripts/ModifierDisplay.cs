using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifierDisplay : MonoBehaviour
{
    public enum TargetType
    {
        Modifier,
        Output
    }

    [Tooltip("Target type")]
    public TargetType m_targetType;

    [Tooltip("Target object Name")]
    public string m_TargetName;

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
        if (m_targetType == TargetType.Output)
        {
            UpdateOutput();
        }
        else if (m_targetType == TargetType.Modifier)
        {
            UpdateModifier();
        }
    }

    void UpdateModifer()
    {
        m_LastWeapon = m_ActiveWeapon;
        m_ActiveWeapon = m_WeaponsManager.GetActiveWeapon();

        if (m_ActiveWeapon.weaponModifierManager != null){
             m_ModifierManager = m_ActiveWeapon.weaponModifierManager;
            var currentWeaponModifiers = m_ModifierManager.GetWeaponModifiers();
            for (int i = 0; i < currentWeaponModifiers.Count; i++)
            {
                if (currentWeaponModifiers[i].modifierName.ToLower() == m_TargetName.ToLower())
                {
                    m_SelfTMP.SetText(currentWeaponModifiers[i].modifierValue.ToString());
                    break;
                }
            }    
        }
    }

    void UpdateOutput()
    {       
        
    }
}
