using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Gauge_UIDisplay : MonoBehaviour
{
    [Tooltip("(Case Sensitive) Match to ModiferType")]
    public WeaponModifier.ModifierType modType;

    private Color modColor = Color.black;

    private float fullWidth;

    private Slider sliderElement;
    private Image fillImage;
    private Image backgroundImage;
    private GameObject secondarylimitObject;

    private WeaponController currentWeapon;


    void Start()
    {
        sliderElement = GetComponent<Slider>();

        var rect = this.GetComponent<RectTransform>();
        fullWidth = rect.rect.width;

        foreach (Image img in GetComponentsInChildren<Image>())
        {
            if (img.gameObject.name == "Bar")
            {
                fillImage = img.GetComponent<Image>();
            } else if (img.gameObject.name == "Background")
            {
                backgroundImage = img.GetComponent<Image>();
            } else if (img.gameObject.name == "SecondaryLimit")
            {
                secondarylimitObject = img.gameObject;
            }
        }

        GameEvents.current.onWeaponChange += UpdateMaximum;
        GameEvents.current.onGaugeInnerUpdate += UpdateSecondaryMaximum;

        switch (modType)
        {
            case WeaponModifier.ModifierType.Force:
                GameEvents.current.onForceModChange += UpdateGaugeDisplay;
                modColor = Color.yellow;
                break;
            case WeaponModifier.ModifierType.Size:
                GameEvents.current.onSizeModChange += UpdateGaugeDisplay;
                modColor = Color.green;
                break;
            case WeaponModifier.ModifierType.Temperature:
                GameEvents.current.onTempModChange += UpdateGaugeDisplay;
                modColor = Color.red;
                break;
        }

        fillImage.color = modColor;
    }

    public void UpdateGaugeDisplay(float newVal)
    {
        // this needs to happen before the secondary is called
        // events are firing and both calling this and UpdateSecMax depends on this
        // this is race condition
        sliderElement.value = newVal;
        GameEvents.current.GaugeInnerUpdate();
    }

    public void UpdateMaximum(WeaponController newWeapon)
    {
        currentWeapon = newWeapon;
        float max = newWeapon.weaponModifierManager.m_PointsAvailable - 2;
        sliderElement.maxValue = max;
    }

    public void UpdateSecondaryMaximum()
    {
        // newMax represents a distance between current value and the sec max
        var used = currentWeapon.weaponModifierManager.GetCurrentUsedPoints();
        var max = currentWeapon.weaponModifierManager.m_PointsAvailable;

        // range will be 1 -> sliderElement.maxValue
        // this will be in  0 -> fullWidth

        var posx = HelperFunctions.ScaleToRange((max - used) + sliderElement.value, sliderElement.maxValue, 1f, fullWidth, 0f);
        
        secondarylimitObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(posx, 0f);
    }
}
