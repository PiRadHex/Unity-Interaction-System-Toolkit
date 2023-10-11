using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfos
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    [SerializeField] bool isMyCar = false;
    [SerializeField] List<AxleInfos> axleInfos;
    [Range(0f,12000f)] [SerializeField] float maxMotorTorque = 12000f;
    [Range(1f, 1.8f)] [SerializeField] float maxDriftSteeringMultiplier = 1.8f;
    [SerializeField] float maxSteeringAngle = 25f;
    [SerializeField] bool hasBoost = false;
    [SerializeField] float boostMultiplier = 1.6f;
    [SerializeField] float brakeTorque = 18000f;
    [SerializeField] float liftCoefficiet = -0.1f;
    [SerializeField] bool hasAutoRotate = true;
    [SerializeField] float speedLimiterDragForce = 0.04f;
    [SerializeField] int carSpeed = 0;

    private float motor;
    private float steering;
    private float boost;
    private float jump;
    private float resetRotationTime = 0f;
    private bool resetRotationFlag = false;
    private float lift;
    private new Rigidbody rigidbody;
    private bool isUpwardForceEnable = false;
    private bool brakeFlag = false;


    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        motor = Input.GetAxis("Vertical");
        steering = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift) && hasBoost)
        {
            boost = boostMultiplier;
        }
        else
        {
            boost = 1f;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && false)
        {
            isUpwardForceEnable = !isUpwardForceEnable;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            brakeFlag = true;
        }
        else
        {
            brakeFlag = false;
        }

        // Lift
        lift = liftCoefficiet * rigidbody.velocity.sqrMagnitude;

        carSpeed = Mathf.RoundToInt(rigidbody.velocity.sqrMagnitude);
    }

    public void FixedUpdate()
    {
        
        foreach (AxleInfos axleInfo in axleInfos)
        {
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);

            if (axleInfo.leftWheel.isGrounded == true && axleInfo.rightWheel.isGrounded == true)
            {
                rigidbody.AddForce(lift * transform.up, ForceMode.Acceleration);

                // Drag : Max Speed Limit
                rigidbody.AddForce(-rigidbody.velocity.x * speedLimiterDragForce, 0f, -rigidbody.velocity.z * speedLimiterDragForce, ForceMode.Acceleration);

            }
            ////////////////////////
            if (!isMyCar)
            {
                axleInfo.leftWheel.brakeTorque = brakeTorque * 0.8f;
                axleInfo.rightWheel.brakeTorque = brakeTorque * 0.8f;
                axleInfo.leftWheel.steerAngle = 0f;
                axleInfo.rightWheel.steerAngle = 0f;
                continue;
            }

            if (axleInfo.steering)
            {
                float mult = Mathf.Clamp(4 / axleInfo.leftWheel.sidewaysFriction.extremumValue, 1f, maxDriftSteeringMultiplier);
                axleInfo.leftWheel.steerAngle = maxSteeringAngle * steering * mult;
                axleInfo.rightWheel.steerAngle = maxSteeringAngle * steering * mult;
            }
            if (axleInfo.motor)
            {
                if (motor < 0 && axleInfo.leftWheel.rpm < 0 && axleInfo.rightWheel.rpm < 0)
                {
                    motor *= 0.5f;
                }
                axleInfo.leftWheel.motorTorque = maxMotorTorque * motor * boost;
                axleInfo.rightWheel.motorTorque = maxMotorTorque * motor * boost;
            }
            if (isUpwardForceEnable)
            {
                rigidbody.AddForce(Vector3.up * 0.04f, ForceMode.VelocityChange);
            }
            if (brakeFlag == true)
            {
                
                if (Mathf.Abs(motor) < 0.1f || rigidbody.velocity.sqrMagnitude > 200f)
                {
                    axleInfo.leftWheel.brakeTorque = brakeTorque;
                    axleInfo.rightWheel.brakeTorque = brakeTorque;
                    NotDriftHandler(axleInfo);
                }
                else
                {
                    DriftHandler(axleInfo);
                }
                
            }
            else
            {
                NotDriftHandler(axleInfo);
                axleInfo.leftWheel.brakeTorque = 0f;
                axleInfo.rightWheel.brakeTorque = 0f;
            }

            /////////////////////////////////////////////////////////////
            if (hasAutoRotate)
            {
                AutoRotate(axleInfo);
            }



        }


    }

    private void AutoRotate(AxleInfos axleInfo)
    {
        if (axleInfo.leftWheel.isGrounded == false && axleInfo.rightWheel.isGrounded == false)
        {
            axleInfo.leftWheel.motorTorque = 0;
            axleInfo.rightWheel.motorTorque = 0;
            resetRotationTime += Time.deltaTime;
            if (resetRotationTime >= 10)
            {
                rigidbody.AddForce(0f, 5f, 0f, ForceMode.VelocityChange);
                resetRotationFlag = true;
                resetRotationTime = 0f;
            }
            if (resetRotationFlag)
            {
            
                float mult = 3f;
                transform.rotation = new Quaternion(Mathf.MoveTowardsAngle(transform.rotation.x, 0f, Time.deltaTime * mult),
                                                    transform.rotation.y,
                                                    Mathf.MoveTowardsAngle(transform.rotation.z, 0f,Time.deltaTime * mult),
                                                    transform.rotation.w);

                if (transform.rotation.x == 0f && transform.rotation.z == 0f)
                {
                    rigidbody.AddForce(0f, -0.1f, 0f, ForceMode.VelocityChange);
                }
            }

        }
        else
        {
            resetRotationTime = 0f;
            resetRotationFlag = false;
        }
    }

    private void DriftHandler(AxleInfos axleInfo)
    {
        WheelFrictionCurve leftWheel = axleInfo.leftWheel.sidewaysFriction;
        WheelFrictionCurve rightWheel = axleInfo.leftWheel.sidewaysFriction;

        float carV = Mathf.Abs(rigidbody.velocity.sqrMagnitude);
        float rpmV = Mathf.Abs(axleInfo.leftWheel.rpm * axleInfo.leftWheel.rpm);
        float driftFactor = Mathf.Clamp01(carV/100 + rpmV/100) * 10f;

        float sidewaysFriction = 12;
        if (carSpeed < 50)
        {
            sidewaysFriction = 1f;
        }
        else if (carSpeed < 100)
        {
            sidewaysFriction = 2f;
        }
        else if (carSpeed < 150)
        {
            sidewaysFriction = 4f;
        }
        else if (carSpeed < 200)
        {
            sidewaysFriction = 8f;
        }

        axleInfo.leftWheel.brakeTorque = brakeTorque * carSpeed / 200;
        axleInfo.rightWheel.brakeTorque = brakeTorque * carSpeed / 200;

        leftWheel.extremumValue = Mathf.MoveTowards(leftWheel.extremumValue, sidewaysFriction, Time.deltaTime * driftFactor);
        leftWheel.asymptoteValue = Mathf.MoveTowards(leftWheel.asymptoteValue, sidewaysFriction, Time.deltaTime * driftFactor);
        rightWheel.extremumValue = Mathf.MoveTowards(rightWheel.extremumValue, sidewaysFriction, Time.deltaTime * driftFactor);
        rightWheel.asymptoteValue = Mathf.MoveTowards(rightWheel.asymptoteValue, sidewaysFriction, Time.deltaTime * driftFactor);

        axleInfo.leftWheel.sidewaysFriction = leftWheel;
        axleInfo.rightWheel.sidewaysFriction = rightWheel;

    }

    private void NotDriftHandler(AxleInfos axleInfo)
    {
        WheelFrictionCurve leftWheel = axleInfo.leftWheel.sidewaysFriction;
        WheelFrictionCurve rightWheel = axleInfo.leftWheel.sidewaysFriction;

        float sidewaysFriction = 32;
        if (carSpeed < 100) sidewaysFriction = 4;
        else if (carSpeed < 200) sidewaysFriction = 8;
        else if (carSpeed < 300) sidewaysFriction = 16;
        else if (carSpeed < 400) sidewaysFriction = 24;

        float mult = 1 + carSpeed / 50;
        leftWheel.extremumValue = Mathf.MoveTowards(leftWheel.extremumValue, sidewaysFriction, Time.deltaTime * mult);
        leftWheel.asymptoteValue = Mathf.MoveTowards(leftWheel.asymptoteValue, sidewaysFriction, Time.deltaTime * mult);
        rightWheel.extremumValue = Mathf.MoveTowards(rightWheel.extremumValue, sidewaysFriction, Time.deltaTime * mult);
        rightWheel.asymptoteValue = Mathf.MoveTowards(rightWheel.asymptoteValue, sidewaysFriction, Time.deltaTime * mult);

        axleInfo.leftWheel.sidewaysFriction = leftWheel;
        axleInfo.rightWheel.sidewaysFriction = rightWheel;


    }








}