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

    [HideInInspector]
    public float fullWidth;

    protected Slider sliderElement;
    protected Image fillImage;
    protected Image backgroundImage;
    protected GameObject secondarylimitObject;

    void Start()
    {
        RegisterComponents();
        switch (modType)
        {
            case WeaponModifier.ModifierType.Force:
                modColor = Color.yellow;
                break;
            case WeaponModifier.ModifierType.Size:
                modColor = Color.green;
                break;
            case WeaponModifier.ModifierType.Temperature:
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
    }
    
    public void UpdateMaximum(float newMax)
    {
        sliderElement.maxValue = newMax;
    }

    public void UpdateSecondaryMarker(float newSecMax)
    {
        secondarylimitObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(newSecMax, 0f);
    }
}
