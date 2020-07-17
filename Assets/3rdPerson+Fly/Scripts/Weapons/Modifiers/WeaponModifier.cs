using UnityEngine;

public class WeaponModifier : MonoBehaviour
{
    [Tooltip("Modifier Name")]
    public string modifierName; 
    [Tooltip("Modifier Value")]
    public float modifierValue;
    [Tooltip("Modifier Enabled")]
    public bool modifierActive;
    [Tooltip("Modifier Button (see project input)")]
    public string modifierButton;

    public WeaponModifierManager manager;

    private void Start()
    {
        //manager.RegisterModifier(this);
        // if we register in unity editor we don't need to do it on startup, that gives doubles
    }
}