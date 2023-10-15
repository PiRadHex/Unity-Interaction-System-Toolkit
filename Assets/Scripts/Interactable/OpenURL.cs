using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : Interactable
{
    public string urlToOpen = "https://piradhex.itch.io/";

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);

        Application.OpenURL(urlToOpen);
    }

}
