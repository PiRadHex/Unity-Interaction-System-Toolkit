using UnityEngine;

public class MaterialSwitch : Interactable
{
    public Material materialToSwitch;
    public Renderer targetRenderer;

    public float timeToReset = 0.2f;
    public bool dontReset = false;

    private bool setOriginal = true;
    private Material originalMaterial;

    public override void Start()
    {
        base.Start();

        if (targetRenderer == null)
        {
            // If the targetRenderer is not assigned, try to find it on the game object.
            targetRenderer = GetComponent<Renderer>();
        }

        if (targetRenderer != null)
        {
            // Store the original material of the target renderer.
            originalMaterial = targetRenderer.material;
        }
        else
        {
            Debug.LogError("No Renderer found on the GameObject or assigned.");
            enabled = false; // Disable the script if there's no renderer to work with.
        }
    }

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);

        targetRenderer.material = setOriginal ? materialToSwitch : originalMaterial;

        setOriginal = !setOriginal;

        if (!dontReset)
        {
            // If not set to not reset, invoke the reset after the specified time.
            Invoke("ResetToOriginalMaterial", timeToReset);
        }
    }

    private void ResetToOriginalMaterial()
    {
        targetRenderer.material = originalMaterial;
        setOriginal = true;
    }
}
