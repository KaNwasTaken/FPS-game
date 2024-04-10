using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    Ray interactRay;
    public LayerMask layerMask;
    public Camera cam;
    [HideInInspector]
    Interactable currentlyHoveringInteractable;
    public TextMeshProUGUI prompt;

    private void Update()
    {
        RaycastHit hitInfo;
        interactRay = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(cam.transform.position, cam.transform.forward);
        Physics.Raycast(interactRay, out hitInfo, 2f, layerMask);

        // Places the current interactable in a public variable to be accessed
        if (hitInfo.collider != null)
        {
            currentlyHoveringInteractable = hitInfo.collider.GetComponent<Interactable>();
            prompt.text = currentlyHoveringInteractable.PromptMessage;
        }
        else
        {
            currentlyHoveringInteractable = null;
            prompt.text = "";
        }

    }

    public void InteractWithCurrentObject()
    {
        if (currentlyHoveringInteractable != null)
        {
            currentlyHoveringInteractable.BaseInteract();
        }
        else return;
    }
}
