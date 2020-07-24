using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalBody : MonoBehaviour
{

    public float initialTemperature;

   [Tooltip("Temperature at which projectile catches fire")]
    public float ignitionTemperature;

    public float Temperature { get; set; }

    void Awake()
    {
        Temperature = initialTemperature;
    }

}
