using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float maxCheckDistance;
    public LayerMask layerMask;
    public GameObject currentInteractableObj;
    public GameObject interactBox;
    public GameObject interactMessage;
    InteractableObject interactableObject;


    void Update()
    {
        // ray
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if (hit.collider.gameObject != currentInteractableObj)
            {
                currentInteractableObj = hit.collider.gameObject;
                interactableObject = hit.collider.GetComponent<InteractableObject>();
            }
        }
        else
        {
            currentInteractableObj = null;
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            interactableObject.Interact();
        }
    }
}
