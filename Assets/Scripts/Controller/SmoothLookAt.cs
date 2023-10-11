using UnityEngine;

public class SmoothLookAt : MonoBehaviour
{
    public Transform target; // Assign the target GameObject in the Unity Editor
    public float rotationSpeed = 5.0f;

    void Update()
    {
        if (target != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = target.position - transform.position;

            // Create a rotation to look at the player
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly interpolate between the current rotation and the desired rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
