using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [SerializeField] Transform grappleHookBarrel;
    [SerializeField] KeyCode inputKey;
    [SerializeField] float torqueConstant = 10000;
    [SerializeField] Transform[] sensorPoints;

    public bool Grappling => grappling;

    Vector3 grapplePoint;
    Rigidbody rb;
    LineRenderer lineRend;
    float radius;
    float radiusModifier = 1.2f;
    bool grappling = false;
    float maxGrappleDistance = 30f;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        lineRend = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            foreach (var sensor in sensorPoints)
            {
                if (Physics.Raycast(sensor.position, sensor.forward, out RaycastHit rayHit, maxGrappleDistance))
                {
                    grapplePoint = rayHit.point;
                    radius = Vector3.Distance(transform.position, grapplePoint) * radiusModifier;
                    grappling = true;
                    break;
                }
            }
        }
        if (Input.GetKeyUp(inputKey))
        {
            grappling = false;
            lineRend.SetPosition(0, grappleHookBarrel.position);
            lineRend.SetPosition(1, grappleHookBarrel.position);
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

    private void LateUpdate()
    {
        if (Grappling)
        {
            // visuals
            lineRend.SetPosition(0, grapplePoint);
            lineRend.SetPosition(1, grappleHookBarrel.position);
        }
    }
}
