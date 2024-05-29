using System;
using System.Collections;
using System.Collections.Generic;
using MovableObject;
using UnityEngine;

public class Elevator : MovingObject
{
    private string state = "down";
    private GameObject pilone;
    private float distanceBtwWaypoints;

    private void Start()
    {
        pilone = GameObject.Find("Pilone");
        distanceBtwWaypoints = Vector3.Distance(waypoints[0].transform.position, waypoints[1].transform.position);
        
        //Set la taille/pos du pilone selon la distance des points
        Vector3 scale = pilone.transform.localScale;
        Vector3 position = pilone.transform.position;
        float newY = distanceBtwWaypoints;
        pilone.transform.localScale = new Vector3(
            scale.x,
            distanceBtwWaypoints*2,
            scale.z
        );
        pilone.transform.position = new Vector3(
            position.x,
            position.y - distanceBtwWaypoints/2,
            position.z
        );
    }

    public void MoveElevator(Component sender, object data)
    {
        //Ne pas relancer cette fonction tant que l'ascenseur bouge
        if (moving) return;
        
        moving = true;
        movingBehaviour = MovingBehaviour.TRANSLATE;
        
        //Décide dans quelle direction bouger
        if (state == "down")
        {
            nextWaypoint = waypoints[1];    //Vise le waypoint END
            state = "up";
        }
        else if (state == "up")
        {
            nextWaypoint = waypoints[0];    //Vise le waypoint START
            state = "down";
        }
        else throw new Exception("Unknow move state for Elevator.");
    }
}
