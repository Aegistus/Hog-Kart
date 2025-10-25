using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMarks : MonoBehaviour
{
    [SerializeField] WheelCollider wheel;

    readonly float lateralSlipThreshold = .25f;
    TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        trail.emitting = true;
    }

    private void Update()
    {
        wheel.GetGroundHit(out WheelHit hit);
        if (hit.collider != null && Mathf.Abs(hit.sidewaysSlip) > lateralSlipThreshold)
        {
            trail.emitting = true;
        }
        else
        {
            trail.emitting = false;
        }
    }
}
