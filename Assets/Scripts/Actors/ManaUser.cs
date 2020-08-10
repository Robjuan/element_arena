using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaUser : MonoBehaviour
{
    [Tooltip("max mana")]
    public float maxMana;
    private float currentMana;

    [Tooltip("mana regen per tick")]
    public float manaRegen;
    [Tooltip("time per tick")]
    public float manaRegenDelay;
    private float lastManaRegenTime = Mathf.NegativeInfinity;

    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        RegenMana();
    }

    private bool CanRegenMana(float delta)
    {
        bool timing = false;
        bool maxed = false;

        if ((lastManaRegenTime + manaRegenDelay) < Time.time)
        {
            timing = true;
        }
        if (currentMana + delta <= maxMana)
        {
            maxed = true;
        }

        return timing && maxed;
    }
    private void RegenMana()
    {
        // per second ticking
        if (CanRegenMana(manaRegen))
        {
            currentMana += manaRegen;
            GameEvents.current.ManaChange(currentMana);
            lastManaRegenTime = Time.time;
        }
    }

    public float GetCurrentMana()
    {
        return currentMana;
    }

    public float UpdateCurrentMana(float delta)
    {
        currentMana += delta;
        return currentMana;
    }
}
