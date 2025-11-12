using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTestCar : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            rb.AddForce(speed * Time.deltaTime * transform.forward);
        }
    }
}
