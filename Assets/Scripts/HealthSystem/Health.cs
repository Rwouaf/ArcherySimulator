using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Health
{
    /**
     * adds health points
     * @returns float - The remaining health
     */
    public float Add(float value);
    
    /**
     * removes health points
     * @returns float - The remaining health 
     */
    public float Remove(float value);
    
    /**
     * Set health points to a specific value.
     * Warning: May raise an error if the set value is above maximum health 
     */
    public void Set(float value);
    
    /**
     * Set the number of hp to 0
     */
    public void Kill();

    /**
     * Returns the maximum health value
     */
    public float GetMax();

    public float GetHealth();
}
