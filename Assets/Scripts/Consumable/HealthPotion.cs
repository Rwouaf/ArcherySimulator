using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : EventLauncherAbstract
{
    [SerializeField] private float heal;
    
    public override void Launch()
    {
        gameEvent.Raise(null, heal);
    }
}
