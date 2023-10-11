using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class RootMotionControl : MonoBehaviour
{
    private ThirdPersonCharacter thirdPersonCharacter;
    private bool defaultRootMotion;

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        defaultRootMotion = thirdPersonCharacter.applyRootMotion;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Obstacle")
        {
            thirdPersonCharacter.applyRootMotion = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Obstacle")
        {
            thirdPersonCharacter.applyRootMotion = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Obstacle")
        {
            thirdPersonCharacter.applyRootMotion = defaultRootMotion;
        }
    }


}
