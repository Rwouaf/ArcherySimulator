using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Levier : MonoBehaviour
{
    public GameEvent onLevierIsTrigger;
    public HingeJoint tige;
    
    // Update is called once per frame
    void Update()
    {
        //Plus l'angle est élevé plus le levier est baissé
        float angle = tige.angle;
        
        //Si angle > 45deg alors on raise l'event pour faire monter l'ascenceur
        if (angle >= 45f)
        {
            onLevierIsTrigger.Raise();
        }
    }
}
