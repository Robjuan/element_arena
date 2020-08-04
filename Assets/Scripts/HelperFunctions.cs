using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static float ScaleToRange(float x, float existing_max, float existing_min, float target_max, float target_min)
    {
        return (target_max - target_min) * ((x - existing_min) / (existing_max - existing_min)) + target_min;
    }

}
