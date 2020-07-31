using System.Collections;
using System;
using UnityEngine;

public class Gauge_UIDisplay : MonoBehaviour
{
    [Tooltip("(Case Sensitive) Match to ModiferType")]
    public WeaponModifier.ModifierType modType;

    private float modifierMax = 50f;

    // these are set based on the sprite
    private const float ZERO_ANGLE = 183;
    private const float MAX_ANGLE = -81;

    private Transform needleTransform;

    // Start is called before the first frame update
    void Start()
    {
        needleTransform = transform.Find("needle");

        GameEvents.current.onWeaponChange += UpdateMaximum;

        switch (modType)
        {
            case WeaponModifier.ModifierType.Force:
                GameEvents.current.onForceModChange += UpdateGaugeDisplay;
                break;
            case WeaponModifier.ModifierType.Size:
                GameEvents.current.onSizeModChange += UpdateGaugeDisplay;
                break;
            case WeaponModifier.ModifierType.Temperature:
                GameEvents.current.onTempModChange += UpdateGaugeDisplay;
                break;
        }
    }

    void UpdateGaugeDisplay(float newVal)
    {
        // need to know max also
        float totalAngleSize = ZERO_ANGLE - MAX_ANGLE;
        float normalised = newVal / modifierMax;

        needleTransform.eulerAngles = new Vector3(0, 0, ZERO_ANGLE - normalised * totalAngleSize);
    }

    void UpdateMaximum(WeaponController newWeapon)
    {
        float max = newWeapon.weaponModifierManager.m_PointsAvailable;
        Debug.Log(max);
        // called when a new weapon is activated (and thus new weaponmods, and thus new maximums)
        modifierMax = max;
    }
}
