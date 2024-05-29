using System.Collections;
using System.Collections.Generic;
using MovableObject;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    [SerializeField] protected MovingBehaviour movingBehaviour;
    [SerializeField] protected GameObject[] waypoints;
    [SerializeField] private float speed = .5f;
    protected GameObject nextWaypoint;
    protected bool moving = false;

    // Update is called once per frame
    void Update()
    {
        //L'objet doit actuellement se déplacer ?
        if (moving == false) return;
        
        //De quelle manière
        switch (movingBehaviour)
        {
            case MovingBehaviour.TRANSLATE:
                Translate();
                break;
            case MovingBehaviour.PATROL:
                break;
        }
    }

    void Translate()
    {
        //Stop le déplacement si on est arrivé a la destination
        if (Vector3.Distance(transform.position,  nextWaypoint.transform.position) < .1f)
        {
            moving = false;
            return;
        }
        
        //Déplace doucement l'objet vers la destination
        transform.position = Vector3.MoveTowards(
            transform.position,
            nextWaypoint.transform.position,
            speed * Time.deltaTime
        );
    }
}
