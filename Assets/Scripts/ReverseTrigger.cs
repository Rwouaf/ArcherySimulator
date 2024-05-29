using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseTrigger : MonoBehaviour
{
    [SerializeField] private float coolDown;
    [SerializeField] private bool continuous = false;

    [SerializeField] private EventLauncherAbstract eventLauncher; // must be of event launcher interface
    
    private bool upRight = true;
    private float timer = 0;

    private void FixedUpdate()
    {
        if (timer > 0) // only update when cooldown has hit 0
        {
            timer -= Time.fixedTime;
            return;
        }

        // Check if item is upside down -> if not early exit
        var up = new Vector3(0,1,0);
        // if up
        if (Vector3.Dot(transform.up, up) > 0)
        {
            if (upRight) return;
            
            // item was reversed before so we start cooldown
            timer = coolDown;
            upRight = true;
            return;
        }
        
        // check if item can trigger
        if (!continuous && !upRight) return;
        
        // Item is upside down
        upRight = false;
        
        Debug.Log("Upside down");
        eventLauncher.Launch();
        
    }
}
