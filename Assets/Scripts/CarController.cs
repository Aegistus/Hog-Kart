using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] Transform cameraHolder;
    [SerializeField] float motorForce = 100f;
    [SerializeField] float brakeForce = 1000f;
    [SerializeField] float speedLimit = 20f;
    [SerializeField] float maxSteerAngle = 30f;
    [SerializeField] float minSteerAngle = 4f;

    [SerializeField] Light[] brakelights;
    /// <summary>
    /// Wheel colliders in order of Front Left, Front Right, Back Left, Back Right.
    /// </summary>
    public WheelCollider[] wheelColliders;
    public Transform[] wheelTransforms;

    float horizontalInput;
    float verticalInput;
    float currentSteerAngle;
    float currentBrakeForce;
    bool isBraking;

    Rigidbody rb;
    float Speed
    {
        get
        {
            var speed = transform.InverseTransformDirection(rb.velocity).z;
            if (speed < .5 && speed > -.5)
            {
                speed = 0;
            }
            return speed;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheelVisuals();
        HandleBrakeLights();
        print(Speed);
    }

    void GetInput()
    {
        var forwardVector = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        horizontalInput = Vector3.SignedAngle(forwardVector, cameraHolder.forward, transform.up);
        if (horizontalInput > 180 - minSteerAngle || horizontalInput < -180 + minSteerAngle)
        {
            horizontalInput = 0;
        }
        horizontalInput = Mathf.Clamp(horizontalInput, -maxSteerAngle, maxSteerAngle);
        horizontalInput = horizontalInput >= -minSteerAngle && horizontalInput <= minSteerAngle ? 0f : horizontalInput;
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetMouseButton(1);
    }

    void HandleMotor()
    {
        if (Speed > speedLimit)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = 0f;
            }
            return;
        }
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = verticalInput * motorForce;
        }
        //wheelColliders[0].motorTorque = verticalInput * motorForce;
        //wheelColliders[1].motorTorque = verticalInput * motorForce;

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    void ApplyBraking()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].brakeTorque = currentBrakeForce;
        }
    }

    void HandleSteering()
    {
        currentSteerAngle = horizontalInput;
        if (Speed < 0 || Vector3.Dot(Vector3.up, transform.up) < 0)
        {
            currentSteerAngle = -currentSteerAngle;
        }
        // front wheels
        wheelColliders[0].steerAngle = currentSteerAngle;
        wheelColliders[1].steerAngle = currentSteerAngle;

        // back wheels
        wheelColliders[2].steerAngle = -currentSteerAngle;
        wheelColliders[3].steerAngle = -currentSteerAngle;
    }

    void UpdateWheelVisuals()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheelTransforms[i].SetPositionAndRotation(pos, rot);
        }
    }

    void HandleBrakeLights()
    {
        if (isBraking)
        {
            for (int i = 0; i < brakelights.Length; i++)
            {
                brakelights[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < brakelights.Length; i++)
            {
                brakelights[i].enabled = false;
            }
        }
    }
}

