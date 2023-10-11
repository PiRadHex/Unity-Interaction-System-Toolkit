using UnityEngine;

public class ChangeTransform : Interactable
{
    [Header("Auto?")]
    [SerializeField] bool hasOriginalAutoSet = false;
    [SerializeField] bool hasSecondaryAutoSet = false;
    [SerializeField] float INTERACT_DELAY = 0.0f;
    [SerializeField] float AUTO_REACT_DELAY = 2.0f; // Time for auto open/close
    [SerializeField] float TIME_INTERVAL = 0.0f; // Time for auto open/close

    private float autoOpenTimer = 0.0f;
    private float autoCloseTimer = 0.0f;
    private float interactDelayTimer = 0.0f;
    private float intervalTimer = 0.0f;
    private bool notNow = false;

    [Space]
    [SerializeField] bool setOriginal = true;

    [Header("Position")]
    [SerializeField] Vector3 floorDistance = Vector3.zero;
    [SerializeField] float moveSpeed = 1.0f;

    private Vector3 originalPosition = Vector3.zero;

    [Header("Rotation")]
    [SerializeField] Vector3 rotationAmount = Vector3.zero;
    [SerializeField] float rotateSpeed = 1.0f;

    private Vector3 originalRotation = Vector3.zero;

    [Header("Scale")]
    [SerializeField] float desiredCoefficient = 1.0f;
    [SerializeField] float scaleSpeed = 1.0f;

    private Vector3 originalScale;


    public override void Start()
    {
        base.Start();
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation.eulerAngles;
        originalScale = transform.localScale;
    }

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);

        doToggle();
    }

    private void doToggle()
    {
        if (notNow)
        {
            return;
        }

        // Toggle the elevator between 1st Floor and 2nd Floor
        setOriginal = !setOriginal;

        interactDelayTimer = INTERACT_DELAY;
        intervalTimer = TIME_INTERVAL;

    }

    void Update()
    {

        intervalTimer -= Time.deltaTime;
        if (intervalTimer > 0)
        {
            notNow = true;
        }
        else
        {
            notNow = false;
            intervalTimer = 0;
        }

        interactDelayTimer -= Time.deltaTime;
        if (interactDelayTimer > 0)
        {
            return;
        }
        else
        {
            interactDelayTimer = 0;
        }


        //////////////////////////
        HandlePosition();
        HandleRotation();
        HandleScale();
        //////////////////////////
        HandleOriginalAutoSet();
        HandleSecondaryAutoSet();


    }


    private void HandlePosition()
    {
        /*=-=-=-=-=-=-=-=-=
            => Position
        =-=-=-=-=-=-=-=-=*/

        // Calculate the target position based on whether the elevator is in 1st floor or 2nd floor
        Vector3 targetPosition = setOriginal ? originalPosition : originalPosition + floorDistance;

        // Smoothly move the elevator towards the target position
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);


    }

    private void HandleRotation()
    {
        /*=-=-=-=-=-=-=-=-=
            => Rotation
        =-=-=-=-=-=-=-=-=*/

        // Calculate the target angle based on whether the door is open or closed
        Vector3 targetRotation = setOriginal ? originalRotation : originalRotation + rotationAmount;

        // Smoothly rotate the door towards the target angle
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetRotation), rotateSpeed * Time.deltaTime);


    }

    private void HandleScale()
    {
        /*=-=-=-=-=-=-=-=-=
            => Scale
        =-=-=-=-=-=-=-=-=*/

        // Calculate the target scale based on whether the scale is original or desired
        Vector3 targetScale = setOriginal ? originalScale : desiredCoefficient * originalScale;

        // Smoothly change the scale towards the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);


    }


    private void HandleOriginalAutoSet()
    {
        // Auto open/close feature
        if (!hasOriginalAutoSet) return;
        if (setOriginal)
        {
            autoOpenTimer = AUTO_REACT_DELAY;
        }
        else if (!setOriginal && autoOpenTimer <= 0.0f) // Timer expired
        {
            doToggle();
            autoOpenTimer = 0;
        }
        else if (!setOriginal) // Countdown to auto close
        {
            autoOpenTimer -= Time.deltaTime;
        }

    }

    private void HandleSecondaryAutoSet()
    {
        // Auto open/close feature
        if (!hasSecondaryAutoSet) return;
        if (!setOriginal)
        {
            autoCloseTimer = AUTO_REACT_DELAY;
        }
        else if (setOriginal && autoCloseTimer <= 0.0f) // Timer expired
        {
            doToggle();
            autoCloseTimer = 0;
        }
        else if (setOriginal) // Countdown to auto close
        {
            autoCloseTimer -= Time.deltaTime;
        }

    }


}
