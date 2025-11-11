using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarController : MonoBehaviour
{
    public event Action OnReset;
    public event Action<bool> OnInputDisabledChange;

    public bool InputDisabled { get; private set; } = false;
    public float BoostCooldown => boostCooldown;
    public float CurrentBoostCooldown { get; private set; }
    public float SpeedLimit => speedLimit;

    [SerializeField] Transform cameraHolder;
    [SerializeField] float motorForce = 100f;
    [SerializeField] float powerslideForce = 1000f;
    [SerializeField] float speedLimit = 20f;
    [SerializeField] float maxSteerAngle = 30f;
    [SerializeField] float minSteerAngle = 4f;
    [SerializeField] float boostForce = 1000f;
    [SerializeField] float boostCooldown = 2f;

    [SerializeField] GameObject[] brakeLights;
    [SerializeField] Material brakeLightOnMat;
    [SerializeField] Material brakeLightOffMat;
    [SerializeField] ParticleSystem[] boostExhaustParticles;
    /// <summary>
    /// Wheel colliders in order of Front Left, Front Right, Back Left, Back Right.
    /// </summary>
    [SerializeField] WheelCollider[] wheelColliders;
    [SerializeField] Transform[] wheelTransforms;
    [SerializeField] AudioSource boostAudio;

    List<MeshRenderer> brakeLightMeshes = new();
    List<Light> brakeLightLights = new();
    Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    float currentSteerAngle;
    bool isPowersliding;
    bool isBraking;
    float defaultWheelForwardStiffness;

    public float Speed
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
    public bool Frozen { get; private set; } = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < brakeLights.Length; i++)
        {
            brakeLightMeshes.Add(brakeLights[i].GetComponent<MeshRenderer>());
            brakeLightLights.Add(brakeLights[i].GetComponentInChildren<Light>());
        }
        defaultWheelForwardStiffness = wheelColliders[0].forwardFriction.stiffness;
    }

    private void Update()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheelVisuals();
        HandleBrakeLights();
        if (CurrentBoostCooldown > 0)
        {
            CurrentBoostCooldown = Mathf.Clamp(CurrentBoostCooldown - Time.deltaTime, 0, BoostCooldown);
        }
    }

    void GetInput()
    {
        if (InputDisabled)
        {
            verticalInput = 0;
            horizontalInput = 0;
            return;
        }
        var forwardVector = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        horizontalInput = Vector3.SignedAngle(forwardVector, cameraHolder.forward, transform.up);
        if (horizontalInput > 180 - minSteerAngle || horizontalInput < -180 + minSteerAngle)
        {
            horizontalInput = 0;
        }
        horizontalInput = Mathf.Clamp(horizontalInput, -maxSteerAngle, maxSteerAngle);
        horizontalInput = horizontalInput >= -minSteerAngle && horizontalInput <= minSteerAngle ? 0f : horizontalInput;
        verticalInput = Input.GetAxis("Vertical");
        isPowersliding = Input.GetMouseButton(1);
        isBraking = Input.GetKey(KeyCode.Space);
        if (Input.GetMouseButtonDown(0))
        {
            Boost();
        }
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

        ApplyBraking();
        ApplyPowerslide();
    }

    void ApplyBraking()
    {
        // any brake torque greater than 0 will full brake.
        var brakeTorque = isBraking ? 1 : 0;
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].brakeTorque = brakeTorque;
        }
        if (Frozen && isBraking)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = 0;
            }
        }
    }

    void ApplyPowerslide()
    {
        var currentPowerslideForce = isPowersliding ? powerslideForce : .1f;
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].wheelDampingRate = currentPowerslideForce;
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
        if (isBraking || isPowersliding)
        {
            for (int i = 0; i < brakeLights.Length; i++)
            {
                brakeLightLights[i].enabled = true;
                brakeLightMeshes[i].material = brakeLightOnMat;
            }
        }
        else
        {
            for (int i = 0; i < brakeLights.Length; i++)
            {
                brakeLightLights[i].enabled = false;
                brakeLightMeshes[i].material = brakeLightOffMat;
            }
        }
    }

    public void Boost()
    {
        if (CurrentBoostCooldown > 0)
        {
            print("On Cooldown");
            return;
        }
        if (Frozen)
        {
            rb.AddForce(-transform.up * boostForce);
        }
        else
        {
            rb.AddForce(transform.forward * boostForce);
        }
        boostAudio.Play();
        for (int i = 0; i < boostExhaustParticles.Length; i++)
        {
            boostExhaustParticles[i].Play();
        }
        CurrentBoostCooldown = boostCooldown;
    }

    public void ResetToLastCheckpoint()
    {
        rb.velocity = Vector3.zero;
        OnReset.Invoke();
    }

    public void SetCarFrozen(bool frozen)
    {
        Frozen = frozen;
        if (frozen)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                var forwardFric = wheelColliders[i].forwardFriction;
                forwardFric.stiffness = .01f;
                wheelColliders[i].forwardFriction = forwardFric;
            }
        }
        else
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                var forwardFric = wheelColliders[i].forwardFriction;
                forwardFric.stiffness = defaultWheelForwardStiffness;
                wheelColliders[i].forwardFriction = forwardFric;
            }
        }
    }

    public void SetInputDisabled(bool disabled)
    {
        InputDisabled = disabled;
        OnInputDisabledChange?.Invoke(disabled);
    }
}

