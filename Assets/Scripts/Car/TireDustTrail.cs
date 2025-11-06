using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireDustTrail : MonoBehaviour
{
    WheelCollider wheel;
    ParticleSystem particles;

    private void Awake()
    {
        wheel = GetComponentInParent<WheelCollider>();
        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (wheel.isGrounded)
        {
            if (!particles.isPlaying)
            {
                particles.Play();
            }
        }
        else
        {
            if (particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }
}
