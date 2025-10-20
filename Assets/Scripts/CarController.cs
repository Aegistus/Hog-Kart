using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] Transform cameraHolder;
    [SerializeField] float motorForce = 100f;
    [SerializeField] float brakeForce = 1000f;
    [SerializeField] float maxSteerAngle = 30f;
    [SerializeField] float minSteerAngle = 4f;

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

    private void Update()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheelVisuals();
    }

    void GetInput()
    {
        var forwardVector = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        horizontalInput = Vector3.SignedAngle(forwardVector, cameraHolder.forward, Vector3.up);
        horizontalInput = Mathf.Clamp(horizontalInput, -maxSteerAngle, maxSteerAngle);
        horizontalInput = horizontalInput >= -minSteerAngle && horizontalInput <= minSteerAngle ? 0f : horizontalInput;
        print(horizontalInput);
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetMouseButton(1);
    }

    void HandleMotor()
    {
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
}

