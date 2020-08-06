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
        GameEvents.current.onManaCostChange += UpdateSecondaryMarker;
        GameEvents.current.onManaChange += UpdateGaugeDisplay;
    }

    private void UpdateMaxMana(WeaponController newWeapon)
    {
        UpdateMaximum(newWeapon.playerMana.maxMana);
    }

}
