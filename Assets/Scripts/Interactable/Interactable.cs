using System.Collections.Generic;
using UnityEngine;

/*	
	This component is for all objects that the player can
	interact with such as enemies, items etc. It is meant
	to be used as a base class.
*/

public class Interactable : MonoBehaviour
{

    public float radius = 0.25f;               // How close do we need to be to interact?
    public Transform interactionTransform;  // The transform from where we interact in case you want to offset it

    LayerMask interactableMask;

    bool isFocus = false;   // Is this interactable currently being focused?
    Transform player;       // Reference to the player transform

    bool hasInteracted = false; // Have we already interacted with the object?

    //[SerializeField] List<Outline> outlines;

    public virtual void Start()
    {
        //DisableOutlines();
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public virtual void Interact(Transform interactingObjectTransform)
    {
        // This method is meant to be overwritten
        Debug.Log(interactingObjectTransform.name + " has interacted with " + transform.name);

        // Delay the disabling of outlines by 0.5 seconds
        //Invoke("DisableOutlines", 0.5f);
    }

    // Called when the object starts being focused
    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
        //EnableOutlines();
    }

    // Called when the object is no longer focused
    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
        //DisableOutlines();
    }

    /// <summary>
    /// Checks the distance between the player and the interactable object in the Update method. 
    /// This check is necessary because the Interact method is called within this method.
    /// </summary>
    public void checkDistance()
    {
        // If we are currently being focused
        // and we haven't already interacted with the object
        if (isFocus && !hasInteracted)
        {
            // If we are close enough
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            Vector3 direction = (transform.position - player.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            float angle = Mathf.Abs(player.rotation.eulerAngles.y - lookRotation.eulerAngles.y);
            if ((distance <= radius) && (angle <= 5))
            {
                // Interact with the object
                Interact(player);
                hasInteracted = true;
            }
        }
    }

    // Draw our radius in the editor
    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    /*
    private void DisableOutlines()
    {
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
    }

    private void EnableOutlines()
    {
        foreach (Outline outline in outlines)
        {
            outline.enabled = true;
        }
    }
    */

}