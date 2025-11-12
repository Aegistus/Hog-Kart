using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [SerializeField] Transform leftGrapplePoint;

    Vector3 grapplePoint;
    Rigidbody rb;
    float radius;
    float radiusModifier = 1.5f;
    bool grappling = false;
    float maxGrappleDistance = 30f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Physics.Raycast(leftGrapplePoint.position, leftGrapplePoint.forward, out RaycastHit rayHit, maxGrappleDistance))
            {
                grapplePoint = rayHit.point;
                radius = Vector3.Distance(transform.position, grapplePoint) * radiusModifier;
                grappling = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
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
            //Vector3 newForward = (transform.position + rb.velocity).normalized;
            //Quaternion lookRotation = Quaternion.LookRotation(newForward, Vector3.up);
            //rb.MoveRotation(lookRotation);
            rb.AddTorque(-10000f * newVelocity.magnitude * transform.up);
            rb.velocity = newVelocity;
        }
    }
}
