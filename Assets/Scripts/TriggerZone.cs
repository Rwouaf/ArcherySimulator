using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
public class TriggerZone : MonoBehaviour
{
    //Tag de l'objet a vérifier
    public string targetTag;
    //Event to raise
    [SerializeField] private GameEvent onTriggerEvent;
    [SerializeField] private string sceneToTriggerIfLevelSelection = null;
    [SerializeField] private bool RaiseEvent = true;
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == targetTag)
        {
            if (sceneToTriggerIfLevelSelection != null)
                onTriggerEvent.Raise(sceneToTriggerIfLevelSelection);
            if(RaiseEvent)
                onTriggerEvent.Raise();

        }
    }
}
