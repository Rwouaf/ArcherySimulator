using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBasic : MonoBehaviour, Health
{
    public float health;
    public float maxHealth;
    public GameEvent onHealthGain;
    public GameEvent onHealthLoss;
    public GameEvent onDeath;

    // Start is called before the first frame update
    void Start()
    {
        if (health > maxHealth)
        {
            Debug.LogError("Health has been set to a greater value than maxHealth in the inspector");
        }

        if (health <= 0)
        {
            Debug.LogError("Health has been set to 0 or less in the inspector (game object should be alive on start ?)");
        }

        if (maxHealth <= 0)
        {
            Debug.LogError("maxHealth has been set to 0 or less in the inspector (game object should be alive on start ?)");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float Add(float value)
    {
        Debug.Assert(value >= 0, "Adding health should not be done using negative values, use Remove instead. Unexpected behaviors will occur.");
        
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (onHealthGain != null)
        {
            onHealthGain.Raise(this, value);
        }
        
        return health;
    }

    public float Remove(float value)
    {
        Debug.Assert(value >= 0, "Removing health should not be done using negative values, use Add instead. Unexpected behaviors will occur.");
        health -= value;
        if (onHealthLoss != null)
        {
            onHealthLoss.Raise(this, value);
        }
        CheckDeath();
        return health;
    }

    public void Set(float value)
    {
        Debug.Assert(value > maxHealth, "Health value must be less than the maximum health. Unexpected behaviors will occur.");
        health = value;
        CheckDeath();
    }

    public void Kill()
    {
        health = 0;
        CheckDeath();
    }

    public float GetMax()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    private void CheckDeath()
    {
        if (health <= 0 && onDeath != null)
        {
            onDeath.Raise(this, null);
        }
    }
}
