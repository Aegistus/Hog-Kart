using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [SerializeField] Transform grappleHookBarrel;
    [SerializeField] KeyCode inputKey;
    [SerializeField] float torqueConstant = 10000;

    Vector3 grapplePoint;
    Rigidbody rb;
    float radius;
    float radiusModifier = 1.5f;
    bool grappling = false;
    float maxGrappleDistance = 30f;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            if (Physics.Raycast(grappleHookBarrel.position, grappleHookBarrel.forward, out RaycastHit rayHit, maxGrappleDistance))
            {
                grapplePoint = rayHit.point;
                radius = Vector3.Distance(transform.position, grapplePoint) * radiusModifier;
                grappling = true;
            }
        }
        if (Input.GetKeyUp(inputKey))
        {
            grappling = false;
        }
    }

    private void FixedUpdate()
    {
        if (grappling && Vector3.Distance(transform.position, grapplePoint) > radius)
        {
            Vector3 originalVelocity = rb.velocity;
            Vector3 pointOnCurve = transform.position - grapplePoint;
            Vector3 tangentVector = Vector3.Cross(pointOnCurve, Vector3.up).normalized;
            Vector3 newVelocity = Vector3.Project(originalVelocity, tangentVector);
            rb.AddTorque(torqueConstant * newVelocity.magnitude * transform.up);
            rb.velocity = newVelocity;
        }
    }
}
