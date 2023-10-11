using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    public Interactable focus;

    public LayerMask movementMask;
    public LayerMask interactableMask;

    public bool Ragdoll = false;

    [SerializeField] private Transform ragdollRoot;

    Camera cam;
    PlayerMotor motor;
    Vector3 destination;
    bool isButtonDownOnInteractable = false;

    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    // Get references
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        destination = transform.position;

        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();

        SetNPC(Ragdoll);
        if (Ragdoll)
        {
            enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

        //if (EventSystem.current.IsPointerOverGameObject())
            //return;

        // If we press left mouse
        if (Input.GetMouseButtonDown(0))
        {
            // We create a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits Interactable
            if (Physics.Raycast(ray, out hit, 100, interactableMask))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null && Mathf.Abs(hit.point.y - transform.position.y) < 15)
                {
                    SetFocus(interactable);
                    destination = interactable.interactionTransform.position;
                    //destination = new Vector3(8888, 8888, 8888);
                    isButtonDownOnInteractable = true;
                }
            }
        }

        if (Input.GetMouseButton(0) && !isButtonDownOnInteractable)
        {
            // We create a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits Ground
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                if (Mathf.Abs(hit.point.y - transform.position.y) < 15)
                {
                    destination = hit.point;
                    RemoveFocus();
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isButtonDownOnInteractable = false;
        }

        if (/*destination != new Vector3(8888, 8888, 8888) && */focus == null)
        {
            motor.MoveToPoint(destination);   // Move to where we hit
        }
        else
        {
            focus.checkDistance();
        }

    }

    // Set our focus to a new focus
    void SetFocus(Interactable newFocus)
    {
        // If our focus has changed
        if (newFocus != focus)
        {
            // Defocus the old one
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;   // Set our new focus
            motor.FollowTarget(newFocus);   // Follow the new focus
        }

        newFocus.OnFocused(transform);
    }

    // Remove our current focus
    public void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        motor.StopFollowingTarget();
    }

    public void SetNPC(bool _mode = true)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = !_mode;

        PlayerMotor _motor = GetComponent<PlayerMotor>();
        if (_motor != null)
        {
            _motor.destinationFlag.gameObject.SetActive(!_mode);
            _motor.enabled = !_mode;
        }


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
            
            if (rig.name == "face" || rig.name == "Head" || rig.name == "LowManHead")
            {
                rig.AddForce(new Vector3(-transform.forward.normalized.x, 1.5f, -transform.forward.normalized.z) * 15, ForceMode.VelocityChange);
            }
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null) animator.enabled = !_mode;

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null) lineRenderer.enabled = !_mode;

    }
}