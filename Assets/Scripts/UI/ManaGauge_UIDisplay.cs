using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaGauge_UIDisplay : Gauge_UIDisplay
{
    private void Start()
    {
        RegisterComponents();
        fillImage.color = Color.blue;
        GameEvents.current.onWeaponChange += UpdateMaxMana;
        GameEvents.current.onManaCostChange += UpdateManaCostMarker;
        GameEvents.current.onManaChange += UpdateGaugeDisplay;
    }

    private void UpdateMaxMana(WeaponController newWeapon)
    {
        UpdateMaximum(newWeapon.playerMana.maxMana);
    }

    private void UpdateManaCostMarker(float newManacost, float maxMana)
    {
        float newSecMax = HelperFunctions.ScaleToRange(newManacost, maxMana, 1f, fullWidth, 0f);
        UpdateSecondaryMarker(newSecMax);
    }

}
