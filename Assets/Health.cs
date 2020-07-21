using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [Tooltip("Maximum amount of health")]
    public float maxHealth = 10f;

    public float currentHealth { get; set; }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal(float healAmount)
    {
        float healthBefore = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void TakeDamage(float damage, GameObject damageSource)
    {
        float healthBefore = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        GameEvents.current.HealthChange(currentHealth, Number_UIDisplay.DisplayTarget.Health);

        HandleDeath();
    }

    public void Kill()
    {
        currentHealth = 0f;
        HandleDeath();
    }

    private void HandleDeath()
    {
        if (currentHealth <= 0f)
        {
            // die
            currentHealth = 420;
        }
    }
}
