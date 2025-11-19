using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedParticles : MonoBehaviour
{
    [SerializeField] float speedThreshold = 20f;

    CarController car;
    ParticleSystem particles;

    private void Start()
    {
        car = FindAnyObjectByType<CarController>();
        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (car.Speed > speedThreshold)
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
