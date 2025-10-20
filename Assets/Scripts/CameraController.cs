using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] float smoothSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.Rotate(rotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime * Vector3.up);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, smoothSpeed * Time.deltaTime);
    }
}
