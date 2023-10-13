using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeVCamPriority : Interactable
{
    public CinemachineVirtualCamera virtualCamera;
    public int targetPriority = 11;
    [Tooltip("Set Priority Or Toggle Priority?")]
    public bool setPriority = false;
    public float timeToReset = 2f;
    public bool dontReset = true;

    private int originalPriority;
    private bool setOriginal = true;

    public override void Start()
    {
        base.Start();

        originalPriority = virtualCamera.Priority;
    }

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);

        virtualCamera.Priority = setOriginal || setPriority ? targetPriority : originalPriority;

        setOriginal = !setOriginal;

        if (!dontReset)
        {
            // If not set to not reset, invoke the reset after the specified time.
            Invoke("ResetToOriginalPriority", timeToReset);
        }
    }

    private void ResetToOriginalPriority()
    {
        virtualCamera.Priority = originalPriority;
        setOriginal = true;
    }

}
