﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarGaugeManager : MonoBehaviour
{
    public List<Gauge_UIDisplay> managedGauges;

    private float rectWidth;
    private float currentWeaponMaxPoints;

    void Start()
    {
        GameEvents.current.onWeaponChange += UpdateMaximumPoints;
        GameEvents.current.onModChange += UpdateAllGaugeDisplay;
    }

    private void UpdateMaximumPoints(WeaponController newWep)
    {
        currentWeaponMaxPoints = newWep.weaponModifierManager.m_PointsAvailable;
        foreach(Gauge_UIDisplay gauge in managedGauges)
        {
            gauge.UpdateMaximum(currentWeaponMaxPoints - 2);
            rectWidth = gauge.fullWidth;
        }
    }

    private void UpdateAllGaugeDisplay(Dictionary<WeaponModifier.ModifierType, float> modDict)
    {
        var totalUsed = 0f;
        foreach(var modval in modDict.Values)
        {
            totalUsed += modval;
        }
        foreach(Gauge_UIDisplay gauge in managedGauges)
        {
            var newVal = modDict[gauge.modType];
            gauge.UpdateGaugeDisplay(newVal);
            
            var scaledVal_secondary = HelperFunctions.ScaleToRange((currentWeaponMaxPoints - totalUsed) + newVal, currentWeaponMaxPoints - 2, 1f, rectWidth, 0f);
            gauge.UpdateSecondaryMarker(scaledVal_secondary);
        }
    }

}
