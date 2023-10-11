using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleInteract : Interactable
{
    // List of MonoBehaviours with an 'interact()' method
    [SerializeField] List<Interactable> interactableComponents = new List<Interactable>();

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);

        // Call the 'interact()' method on all components in the list
        foreach (Interactable component in interactableComponents)
        {
            component.Interact(interactingObjectTransform);
        }

    }


}
