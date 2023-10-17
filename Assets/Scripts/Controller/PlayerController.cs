using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using TMPro;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Interactable focus;

    [Header("Layer")]
    public LayerMask movementMask;
    public LayerMask interactableMask;

    [Header("Mouse Info")]
    public Transform mouseInfoBox;
    public TextMeshProUGUI mouseInfoText;
    public string onRaycastText = "Interactable";
    public Interactable onFocusInteractable;

    [Header("Ragdoll")]
    public bool Ragdoll = false;
    [SerializeField] private Transform ragdollRoot;

    private Camera cam;
    private PlayerMotor motor;
    private Vector3 destination;
    private bool isButtonDownOnInteractable = false;

    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    private NavMeshAgent agent;
    private ThirdPersonCharacter character;
    private new Rigidbody rigidbody;
    private new CapsuleCollider collider;
    private Animator animator;
    private LineRenderer lineRenderer;

    private float rigidbodyMass;

    // Get references
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        destination = transform.position;

        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();

        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();

        rigidbodyMass = rigidbody.mass;

        SetNPC(Ragdoll);
        if (Ragdoll)
        {
            enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        mouseInfoBox.gameObject.SetActive(false);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, interactableMask))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null && Mathf.Abs(hit.point.y - transform.position.y) < 15)
            {
                mouseInfoBox.position = Input.mousePosition + new Vector3(-100 / 2, 18 / 2, 0);
                mouseInfoBox.gameObject.SetActive(true);
                mouseInfoText.text = onRaycastText;
            }
        }

        // If we press left mouse
        if (Input.GetMouseButtonDown(0))
        {
            // If the ray hits Interactable
            if (Physics.Raycast(ray, out hit, 100, interactableMask))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null && Mathf.Abs(hit.point.y - transform.position.y) < 15)
                {
                    SetFocus(interactable);
                    destination = interactable.interactionTransform.position;
                    isButtonDownOnInteractable = true;
                    onFocusInteractable.Interact(transform);
                }
            }
        }

        if (Input.GetMouseButton(0) && !isButtonDownOnInteractable)
        {
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

        if (focus == null)
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
        mouseInfoBox.gameObject.SetActive(!_mode);

        if (agent != null) agent.enabled = !_mode;

        if (motor != null)
        {
            motor.destinationFlag.gameObject.SetActive(!_mode);
            motor.enabled = !_mode;
        }

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
            
            if (rig.name == "face" || rig.name == "Head" || rig.name == "LowManHead")
            {
                rig.AddForce(new Vector3(-transform.forward.normalized.x, 1.5f, -transform.forward.normalized.z) * 15, ForceMode.VelocityChange);
            }
        }

        if (animator != null) animator.enabled = !_mode;

        if (lineRenderer != null) lineRenderer.enabled = !_mode;

    }
}