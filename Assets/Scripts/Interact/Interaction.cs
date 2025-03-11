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
    public GameObject interactMessage;
    InteractableObject interactableObject;


    void Update()
    {
        CheckInteractableObject();
    }

    void CheckInteractableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if (hit.collider.gameObject != currentInteractableObj)
            {
                currentInteractableObj = hit.collider.gameObject;
                interactableObject = hit.collider.GetComponent<InteractableObject>();
                interactMessage.SetActive(true);
            }
        }
        else
        {
            currentInteractableObj = null;
            interactMessage.SetActive(false);
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && currentInteractableObj != null)
        {
            interactableObject.Interact();
        }
    }
}
