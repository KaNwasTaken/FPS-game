using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Objects inheriting from this script can be interacted with

    public virtual string PromptMessage
    {
        get { return "Prompt Message"; }
        set { PromptMessage = value; }
    }


    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {

    }
}
