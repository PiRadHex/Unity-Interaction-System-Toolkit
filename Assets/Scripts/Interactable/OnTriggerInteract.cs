using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerInteract : MonoBehaviour
{
    [SerializeField] bool triggerOnEnter = true;
    [SerializeField] bool triggerOnExit = true;
    [SerializeField] bool disableTriggerAfterInteract = false;
    [SerializeField] bool interactForEachTrigger = true;
    [SerializeField] List<Interactable> interactableComponents;

    [SerializeField] List<string> collisionCheckTags = new List<string> { "Player", "Car", "Portal" };
    [SerializeField] List<string> collisionCheckLayers = new List<string> { "Player", "Car" };
    [SerializeField] List<string> collisionCheckNames = new List<string> { "Player", "Car", "Simple Portal" };

    private HashSet<string> allowedTags;
    private HashSet<string> allowedLayers;
    private HashSet<string> allowedNames;

    private int enteredCount = 0;

    private void Awake()
    {
        // Initialize the hash sets with the allowed tags, layers, and names
        allowedTags = new HashSet<string>(collisionCheckTags);
        allowedLayers = new HashSet<string>(collisionCheckLayers);
        allowedNames = new HashSet<string>(collisionCheckNames);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggerOnEnter) return;

        if (ShouldInteractWith(other))
        {
            Debug.Log(other.gameObject.name + " has entered the " + transform.name + ".");
            Interact(other.transform);
            enteredCount++; // The increment of the enteredCount must occur AFTER the execution of the Interact() function.
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!triggerOnExit) return;

        if (ShouldInteractWith(other))
        {
            Debug.Log(other.gameObject.name + " has exited the " + transform.name + ".");
            enteredCount--; // The decrement of the enteredCount must occur BEFORE the execution of the Interact() function.
            Interact(other.transform);
        }
    }

    private void Interact(Transform interactingObjectTransform)
    {
        if (!IsOkToInteract()) return;

        foreach (Interactable component in interactableComponents)
        {
            component.Interact(interactingObjectTransform);
        }

        if (disableTriggerAfterInteract)
        {
            gameObject.SetActive(false);
        }
    }

    private bool ShouldInteractWith(Collider other)
    {
        string otherTag = other.tag;
        string otherLayer = LayerMask.LayerToName(other.gameObject.layer);
        string otherName = other.gameObject.name;

        return allowedTags.Contains(otherTag) || allowedLayers.Contains(otherLayer) || allowedNames.Contains(otherName);
    }

    private bool IsOkToInteract()
    {
        return interactForEachTrigger || (!interactForEachTrigger && enteredCount == 0);
    }


}
