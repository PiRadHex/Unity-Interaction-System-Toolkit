using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class RandomRunner : MonoBehaviour
{
    public float speedMultiplier = 1f;
    public bool Ragdoll = false;
    
    private NavMeshAgent agent;
    private ThirdPersonCharacter character;

    [SerializeField] private Transform ragdollRoot;
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    [SerializeField] private float maxDestinationRange = 5;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();

        SetNPC(Ragdoll);
        if (Ragdoll)
        {
            enabled = false;
        }

        agent.stoppingDistance = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) SetNPC(true);
        if (Input.GetKeyUp(KeyCode.K)) SetNPC(false);
        if (Ragdoll) return;
        SetRandomDestination();
        UpdateAgentSpeed();
        UpdateAnimationBaseOnVelocity();
    }

    private void SetRandomDestination()
    {
        if (agent.remainingDistance / agent.stoppingDistance < 4)
        {
            Vector3 point = new Vector3(Random.Range(-maxDestinationRange + transform.position.x, maxDestinationRange + transform.position.x),
                                        0, Random.Range(-maxDestinationRange + transform.position.z, maxDestinationRange + transform.position.z));
            agent.SetDestination(point);
        }
    }

    private void UpdateAnimationBaseOnVelocity()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            if (agent.remainingDistance / agent.stoppingDistance > 5)
            {
                character.Move(agent.desiredVelocity / speedMultiplier, false, false);
            }
            else
            {
                character.Move(agent.desiredVelocity / (1 + speedMultiplier) * 2, false, false);
            }
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
    }

    private void UpdateAgentSpeed()
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion lookRotation = direction.x == 0 && direction.z == 0 ? new Quaternion(0f, 0f, 0f, 0f) : Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        if (agent.remainingDistance / agent.stoppingDistance > 10)
        {
            agent.speed = Mathf.Lerp(agent.speed, 1f * speedMultiplier, Time.deltaTime * 1f / speedMultiplier);
        }
        else
        {
            agent.speed = Mathf.Lerp(agent.speed, 0.45f * (1 + speedMultiplier) / 2, Time.deltaTime * 3f);
        }

    }

    public void SetNPC(bool _mode = true)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = !_mode;

        ThirdPersonCharacter character = GetComponent<ThirdPersonCharacter>();
        if (character != null) character.enabled = !_mode;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.mass = _mode ? 0 : 100;
            rigidbody.useGravity = !_mode;
            rigidbody.isKinematic = _mode;
        }

        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        if (collider != null) collider.enabled = !_mode;

        foreach (Collider c in colliders)
        {
            c.enabled = _mode;
        }

        foreach (Rigidbody rig in rigidbodies)
        {
            if (_mode)
            {
                rig.velocity = Vector3.zero;
                rig.detectCollisions = true;
            }
            rig.useGravity = _mode;
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null) animator.enabled = !_mode;

        Ragdoll = _mode;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            SetNPC(true);
        }
    }


}
