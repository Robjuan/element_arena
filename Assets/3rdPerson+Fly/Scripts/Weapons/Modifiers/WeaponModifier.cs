using UnityEngine;

public class WeaponModifier : MonoBehaviour
{
    public enum ModifierType
    {
        None,
        Size,
        Force,
        Temperature
    }

    [Tooltip("Modifier Name")]
    public string modifierName; 
    [Tooltip("Modifier Type")]
    public ModifierType modifierType; 
    [Tooltip("Modifier Value")]
    public float modifierValue;
    [Tooltip("Modifier Scale")]
    public float modifierScale;
    [Tooltip("Modifier Enabled")]
    public bool modifierActive;
    [Tooltip("Modifier Button (see project input)")]
    public string modifierButton;

    public WeaponModifierManager manager;

    public void ApplyModifier(ProjectileBase projectile)
    {
        if (modifierActive)
        {
            if (modifierType == ModifierType.Size)
            {
                float scale = modifierValue * modifierScale;
                projectile.transform.localScale = new Vector3(scale,scale,scale);
                projectile.UpdateMass();
            }

            else if (modifierType == ModifierType.Force)
            {
                float scale = modifierValue * modifierScale;
                projectile.shootForce = scale;
            }

            else if (modifierType == ModifierType.Temperature)
            {
                float scale = modifierValue * modifierScale;
                projectile.temperature = scale;
            }
        }
    }
}