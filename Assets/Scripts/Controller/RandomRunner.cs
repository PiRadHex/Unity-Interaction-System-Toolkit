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
    public bool canBeKilledByButton = false;
    private NavMeshAgent agent;
    private ThirdPersonCharacter character;

    [SerializeField] private Transform ragdollRoot;
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    [SerializeField] private float maxDestinationRange = 5;

    private new Rigidbody rigidbody;
    private new CapsuleCollider collider;
    private Animator animator;

    private float rigidbodyMass;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();

        rigidbodyMass = rigidbody.mass;

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
        if (Input.GetKeyDown(KeyCode.K) & canBeKilledByButton) SetNPC(true);
        if (Input.GetKeyUp(KeyCode.K) & canBeKilledByButton) SetNPC(false);
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
            character.Move(agent.desiredVelocity / speedMultiplier, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
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

    public void SetNPC(bool _mode = true)
    {
        if (agent != null) agent.enabled = !_mode;

        if (character != null) character.enabled = !_mode;

        if (rigidbody != null)
        {
            rigidbody.mass = _mode ? 0 : rigidbodyMass;
            rigidbody.useGravity = !_mode;
            rigidbody.isKinematic = _mode;
        }

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
            rig.interpolation = _mode ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        }

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
