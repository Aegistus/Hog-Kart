using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] float smoothSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;

    CarController car;

    private void Start()
    {
        car = FindAnyObjectByType<CarController>(FindObjectsInactive.Include);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (car.InputDisabled)
        {
            return;
        }
        transform.Rotate(rotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime * Vector3.up);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, smoothSpeed * Time.deltaTime);
    }
}
