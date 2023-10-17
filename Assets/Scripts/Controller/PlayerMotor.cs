using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

/* This component moves our player using a NavMeshAgent. */

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerMotor : MonoBehaviour
{
    public float speedMultiplier = 1f;
    public Transform destinationFlag;

    private Transform moveTarget;   // Target to follow
    private Transform lookTarget;   // Target to look
    private NavMeshAgent agent;     // Reference to our agent
    private ThirdPersonCharacter character;
    private LineRenderer lineRenderer;

    // Get references
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        lineRenderer = GetComponent<LineRenderer>();
        
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        // If we have a target
        if (moveTarget != null)
        {
            // Move towards it and look at it
            MoveToPoint(moveTarget.position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
            
        }

        UpdateAgentSpeed();
        UpdateAnimationBaseOnVelocity();
        
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    // Start following a target
    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * 0.8f;

        moveTarget = newTarget.interactionTransform;
        lookTarget = newTarget.transform;
    }

    // Stop following a target
    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0.2f;

        moveTarget = null;
    }

    // Make sure to look at the target
    void FaceTarget()
    {
        Vector3 direction = (lookTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void UpdateAnimationBaseOnVelocity()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity / speedMultiplier, false, false);
            destinationFlag.gameObject.SetActive(true);
            MoveDestinationFlagAtPosition(agent.destination, false, 5f); // Not sure yet, but keep in mind there is a trade-off between non-teleporting flag movement and displaying the path.
            if (agent.hasPath) DrawPath();
        }
        else
        {
            character.Move(Vector3.zero, false, false);
            //MoveDestinationFlagAtPosition(transform.position, false, 1f);
            destinationFlag.gameObject.SetActive(false);
            lineRenderer.enabled = false;
        }
    }

    private void MoveDestinationFlagAtPosition(Vector3 pos, bool moveSmooth = true, float v = 1f)
    {
        if (moveSmooth)
        {
            destinationFlag.SetParent(null);
            destinationFlag.position = Vector3.Lerp(destinationFlag.position, new Vector3(pos.x, destinationFlag.position.y, pos.z), Time.deltaTime * v);
        }
        else
        {
            destinationFlag.position = new Vector3(pos.x, destinationFlag.position.y, pos.z);
        }
        

        RaycastHit hit;
        if (Physics.Raycast(destinationFlag.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            destinationFlag.position = new Vector3(destinationFlag.position.x, hit.point.y + 0.1f, destinationFlag.position.z);
            destinationFlag.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    private void UpdateAgentSpeed()
    {
        float agentSpeed = agent.speed;

        if (agent.remainingDistance / agent.stoppingDistance > 10)
        {
            if (agentSpeed < speedMultiplier - 0.45f)
            {
                agent.speed = Mathf.Lerp(agentSpeed, 1f * (speedMultiplier + 2), Time.deltaTime * 1f / (agentSpeed + 1));
            }
            else
            {
                agent.speed = Mathf.Lerp(agentSpeed, 1f * (speedMultiplier + 2), Time.deltaTime * 2f);
            }

        }
        else
        {
            agent.speed = Mathf.Lerp(agentSpeed, 0.45f * speedMultiplier, Time.deltaTime * 6f);
        }
        
    }

    private void DrawPath()
    {
        lineRenderer.enabled = true;
        int pathCornersLength = agent.path.corners.Length;
        lineRenderer.positionCount = pathCornersLength;
        lineRenderer.SetPosition(0, transform.position + (agent.path.corners[1] - agent.path.corners[0]).normalized * 0.2f);

        for (int i = 1; i < pathCornersLength; i++)
        {
            
            lineRenderer.SetPosition(i, agent.path.corners[i]);
        }
    }

}