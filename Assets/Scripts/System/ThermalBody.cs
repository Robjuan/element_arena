using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalBody : MonoBehaviour
{
    public float initialTemperature;
    public float ignitionTemperature;

    public float Temperature { get; set; }

    void Awake()
    {
        Temperature = initialTemperature;
    }

    public bool isIgnited()
    {
        return Temperature >= ignitionTemperature;
    }
}
