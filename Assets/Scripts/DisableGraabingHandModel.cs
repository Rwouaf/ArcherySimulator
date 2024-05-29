using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableGraabingHandModel : MonoBehaviour
{

    public GameObject leftHandModel;
    public GameObject rightHandModel;
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        grabInteractable.selectEntered.AddListener(ShowGrabbingHand);

    }

    public void HideGrabbingHand(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.tag == "left Hand")
        {
            leftHandModel.SetActive(false);
            Debug.Log("prada");
        }
        else if (args.interactableObject.transform.tag == "right Hand")
            rightHandModel.SetActive(false);
    }

    public void ShowGrabbingHand(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.tag == "left Hand")
            leftHandModel.SetActive(true);
        else if (args.interactableObject.transform.tag == "right Hand")
            rightHandModel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
