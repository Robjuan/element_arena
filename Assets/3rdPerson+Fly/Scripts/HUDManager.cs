using UnityEngine;
using System.Collections.Generic;

// TODO: event system

public class HUDManager : MonoBehaviour
{
    [Tooltip("Managed HUD Counters")]
    public List<ModifierDisplay> l_counters;

    void Update()
    {
        foreach (var counter in l_counters)
        {
            counter.UpdateSelf();
        }
    }
    
}

    
