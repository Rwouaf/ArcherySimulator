using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HoldPlank : MonoBehaviour
{
    public InteractionLayerMask targetLayer = 0;
    public XRGrabInteractable interactor = null;
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "PlankHolder")
        {
            interactor.interactionLayers = targetLayer;
        }
        
    }
}
