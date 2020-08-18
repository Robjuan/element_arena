using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    public abstract bool IsCompleted();
    public abstract void Complete();
    
}
