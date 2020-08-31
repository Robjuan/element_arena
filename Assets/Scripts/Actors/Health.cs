﻿using UnityEngine;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
        
    [Tooltip("Maximum amount of health")]
    public float maxHealth = 10f;

    public Slider worldVisibleHealthBar;

    private ActorController parent;
    private bool isDead;
    public float currentHealth { get; set; }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        parent = GetComponent<ActorController>();
        
        // initialise HUD
        if (this.gameObject.tag == "Player")
        {
            GameEvents.current.HealthChange(currentHealth);
        }
        if(worldVisibleHealthBar != null)
        {
            worldVisibleHealthBar.maxValue = maxHealth;
            worldVisibleHealthBar.value = currentHealth;
        }

    }

    private void UpdateVisibleHealthBar(float value)
    {
        if(worldVisibleHealthBar != null)
        {
            worldVisibleHealthBar.value = value;
        }
    }

    public void Heal(float healAmount)
    {
        float healthBefore = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateVisibleHealthBar(currentHealth);
    }

    public void TakeDamage(float damage, GameObject damageSource)
    {
        float healthBefore = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (this.gameObject.tag == "Player"){
            GameEvents.current.HealthChange(currentHealth);
        }
        UpdateVisibleHealthBar(currentHealth);
        HandleDeath();
    }

    public void Kill()
    {
        currentHealth = 0f;
        HandleDeath();
    }

    private void HandleDeath()
    {
        if (currentHealth <= 0f && !isDead)
        {
            Debug.Log("Dying actor: " + this.gameObject);
            if (parent)
            {
                parent.Die();
                parent.IsAlive = false;
            }

            if (this.gameObject)
            {
                GameEvents.current.ActorDeath(this.gameObject);
            }
            isDead = true;
        }
    }

    void Update()
    {
        if (worldVisibleHealthBar != null)
        {
            worldVisibleHealthBar.transform.LookAt(Camera.main.transform);
        }
    }
}
