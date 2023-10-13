using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;

public class PlayerSwitch : Interactable
{
    public List<CinemachineVirtualCamera> virtualCameras;
    public bool switchControl = true;
    public List<GameObject> players;

    private int currentCameraIndex = 0;
    private int currentPlayerIndex = 0;

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);
        
        if (virtualCameras[currentCameraIndex].Priority == 10)
        {
            currentCameraIndex = (currentCameraIndex + 1) % virtualCameras.Count;
        }

        CinemachineVirtualCamera virtualCamera1 = virtualCameras[currentCameraIndex];
        currentCameraIndex = (currentCameraIndex + 1) % virtualCameras.Count;
        CinemachineVirtualCamera virtualCamera2 = virtualCameras[currentCameraIndex];

        virtualCamera2.Priority = 11;
        virtualCamera1.Priority = 10;

        if (switchControl)
        {
            if (!players[currentPlayerIndex].GetComponent<PlayerController>().enabled)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            }

            PlayerController player1 = players[currentPlayerIndex].GetComponent<PlayerController>();
            if (player1 != null)
            {
                player1.SetNPC(true);
            }

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            PlayerController player2 = players[currentPlayerIndex].GetComponent<PlayerController>();
            if (player2 != null)
            {
                player2.enabled = true;
                player2.SetNPC(false);
            }

            player1.RemoveFocus();
            player1.enabled = false;
                    

        }

    }


}
