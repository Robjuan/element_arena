using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Gauge_UIDisplay : MonoBehaviour
{
    [Tooltip("(Case Sensitive) Match to ModiferType")]
    public WeaponModifier.ModifierType modType;

    protected Color modColor = Color.black;

    protected float fullWidth;

    protected Slider sliderElement;
    protected Image fillImage;
    protected Image backgroundImage;
    protected GameObject secondarylimitObject;

    protected WeaponController currentWeapon;

    void Start()
    {
        RegisterComponents();

        GameEvents.current.onWeaponChange += UpdateMaximumPoints;

        // firing this when we update the currently changing mod
        // so other two mods can listen and update their secondary maxes
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

    public void RegisterComponents()
    {
        sliderElement = GetComponent<Slider>();

        var rect = this.GetComponent<RectTransform>();
        fullWidth = rect.rect.width;

        foreach (Image img in GetComponentsInChildren<Image>())
        {
            if (img.gameObject.name == "Bar")
            {
                fillImage = img.GetComponent<Image>();
            }
            else if (img.gameObject.name == "Background")
            {
                backgroundImage = img.GetComponent<Image>();
            }
            else if (img.gameObject.name == "SecondaryLimit")
            {
                secondarylimitObject = img.gameObject;
            }
        }
    }

    public void UpdateGaugeDisplay(float newVal)
    {
        sliderElement.value = newVal;
        GameEvents.current.GaugeInnerUpdate();
    }

    public void UpdateMaximumPoints(WeaponController newWeapon)
    {
        Debug.Log("updating max points: "+newWeapon);
        currentWeapon = newWeapon;
        float max = newWeapon.weaponModifierManager.m_PointsAvailable - 2;
        UpdateMaximum(max);
    }


    public void UpdateMaximum(float newMax)
    {
        sliderElement.maxValue = newMax;
    }

    public void UpdateSecondaryMaximum()
    {
        var used = currentWeapon.weaponModifierManager.GetCurrentUsedPoints();
        var max = currentWeapon.weaponModifierManager.m_PointsAvailable;
        UpdateSecondaryMarker((max - used) + sliderElement.value);
    }

    public void UpdateSecondaryMarker(float newSecMax)
    {
        // range will be 1 -> sliderElement.maxValue
        // this will be in  0 -> fullWidth

        var posx = HelperFunctions.ScaleToRange(newSecMax, sliderElement.maxValue, 1f, fullWidth, 0f);
        secondarylimitObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(posx, 0f);
    }
}
