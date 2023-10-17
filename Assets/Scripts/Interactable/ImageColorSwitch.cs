using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorSwitch : Interactable
{
    public Image image;
    public Color targetColor;

    public float timeToReset = 0.2f;
    public bool dontReset = false;

    private Color originalColor;
    private float timeStamp;

    public override void Start()
    {
        base.Start();

        originalColor = image.color;
    }

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);

        timeStamp = Time.time;

        image.color = targetColor;

        if (!dontReset)
        {
            // If not set to not reset, invoke the reset after the specified time.
            Invoke("ResetToOriginalColor", timeToReset);
        }
    }

    private void ResetToOriginalColor()
    {
        if (Time.time - timeStamp >= timeToReset - 0.05)
        {
            image.color = originalColor;
        }
        
    }

}
