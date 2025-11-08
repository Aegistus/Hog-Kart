using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMarks : MonoBehaviour
{
    [SerializeField] WheelCollider wheel;
    [SerializeField] ParticleSystem dust;
    [SerializeField] ParticleSystem frozenDust;

    bool isDrifting = false;
    bool isSpinning = false;
    readonly float lateralSlipThreshold = .25f;
    TrailRenderer trail;
    AudioSource driftAudio;
    CarController car;

    private void Awake()
    {
        car = GetComponentInParent<CarController>();
        car.OnReset += () =>
        {
            trail.emitting = false;
            trail.Clear();
        };
        dust.Stop();
        trail = GetComponent<TrailRenderer>();
        driftAudio = GetComponent<AudioSource>();
        trail.emitting = false;
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
        if (car.Frozen && wheel.rpm > 5)
        {
            if (!isSpinning)
            {
                SpinInPlace(true);
            }
        }
        else
        {
            if (isSpinning)
            {
                SpinInPlace(false);
            }
        }
    }

    void Drift(bool starting)
    {
        isDrifting = starting;
        trail.emitting = starting;
        if (starting)
        {
            driftAudio.pitch = 1 + Random.Range(-.1f, .1f);
            driftAudio.Play();
            dust.Play();
        }
        else
        {
            driftAudio.Stop();
            dust.Stop();
        }
    }

    void SpinInPlace(bool starting)
    {
        isSpinning = starting;
        if (starting)
        {
            frozenDust.Play();
            driftAudio.Play();
        }
        else
        {
            frozenDust.Stop();
            driftAudio.Stop();
        }
    }
}
