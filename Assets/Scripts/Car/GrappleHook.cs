using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [SerializeField] Transform grappleHookBarrel;
    [SerializeField] Transform grappleHookTip;
    [SerializeField] Transform[] sensorPoints;
    [SerializeField] KeyCode inputKey;
    [SerializeField] float torqueConstant = 10000;
    [SerializeField] int quality = 500;
    [SerializeField] float damper = 14;
    [SerializeField] float strength = 800;
    [SerializeField] float velocity = 15;
    [SerializeField] int waveCount = 4;
    [SerializeField] float waveHeight = 1f;
    [SerializeField] AnimationCurve effectCurve;

    public bool Grappling => grappling;

    Vector3 grapplePoint;
    Vector3 currentRopePoint;
    Vector3 hookTipStartPosition;
    Rigidbody rb;
    LineRenderer lineRend;
    Spring spring;
    float radius;
    float radiusModifier = 1.2f;
    bool grappling = false;
    float maxGrappleDistance = 30f;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        lineRend = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
        hookTipStartPosition = grappleHookTip.localPosition;
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
                    currentRopePoint = grappleHookBarrel.position;
                    radius = Vector3.Distance(transform.position, grapplePoint) * radiusModifier;
                    SoundManager.Instance.PlaySoundAtPosition("GrappleHookShoot", transform.position);
                    grappling = true;
                    break;
                }
            }
        }
        if (Input.GetKeyUp(inputKey))
        {
            grappling = false;
            grapplePoint = grappleHookBarrel.position;
            currentRopePoint = grappleHookBarrel.position;
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
        DrawRope();
    }

    void DrawRope()
    {
        if (!Grappling)
        {
            spring.Reset();
            if (lineRend.positionCount > 0)
            {
                lineRend.positionCount = 0;
            }
            grappleHookTip.localPosition = hookTipStartPosition;
        }
        else
        {
            if (lineRend.positionCount == 0)
            {
                spring.SetVelocity(velocity);
                lineRend.positionCount = quality + 1;
            }

            spring.SetDamper(damper);
            spring.SetStrength(strength);
            spring.Update(Time.deltaTime);

            var upVector = Quaternion.LookRotation((grapplePoint - grappleHookBarrel.position).normalized) * Vector3.up;
            currentRopePoint = Vector3.Lerp(currentRopePoint, grapplePoint, Time.deltaTime * velocity);

            for (int i = 0; i < quality + 1; i++)
            {
                var delta = i / (float)quality;
                var offset = effectCurve.Evaluate(delta) * Mathf.Sin(delta * waveCount * Mathf.PI * spring.Value) * waveHeight * upVector;
                lineRend.SetPosition(i, Vector3.Lerp(grappleHookBarrel.position, currentRopePoint, delta) + offset);
            }
            grappleHookTip.position = lineRend.GetPosition(lineRend.positionCount - 1);
        }

    }
}
