using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMarks : MonoBehaviour
{
    [SerializeField] WheelCollider wheel;

    bool isDrifting = false;
    readonly float lateralSlipThreshold = .25f;
    TrailRenderer trail;
    AudioSource driftAudio;

    private void Awake()
    {
        GetComponentInParent<CarController>().OnReset += () =>
        {
            trail.emitting = false;
            trail.Clear();
        };
        trail = GetComponent<TrailRenderer>();
        driftAudio = GetComponent<AudioSource>();
        trail.emitting = true;
    }

    private void Update()
    {
        wheel.GetGroundHit(out WheelHit hit);
        if (hit.collider != null && Mathf.Abs(hit.sidewaysSlip) > lateralSlipThreshold)
        {
            if (!isDrifting)
            {
                Drift(true);
            }
        }
        else
        {
            if (isDrifting)
            {
                Drift(false);
            }
        }
    }

    void Drift(bool starting)
    {
        isDrifting = starting;
        trail.emitting = starting;
        if (starting)
        {
            driftAudio.Play();
        }
        else
        {
            driftAudio.Stop();
        }
    }
}
