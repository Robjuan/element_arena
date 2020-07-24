using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Number_UIDisplay : MonoBehaviour
{
    public enum DisplayTarget
    {
        Health
    }

    [Tooltip("Target Number to Display")]
    public DisplayTarget m_thisDisplayTarget;

    TextMeshProUGUI m_SelfTMP;

    void Start()
    {
        m_SelfTMP = gameObject.GetComponent<TextMeshProUGUI>();
        GameEvents.current.onHealthChange += UpdateNumberDisplay;
    }

    private void UpdateNumberDisplay(float newVal, DisplayTarget numType)
    {
        if (m_thisDisplayTarget == numType)
        {
            m_SelfTMP.SetText(newVal.ToString());
        }
    }
    
}
