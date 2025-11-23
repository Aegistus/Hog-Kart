using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSprayParticles : MonoBehaviour
{
    [SerializeField] int minParticles = 10;
    [SerializeField] int maxParticles = 50;
    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 10f;

    ParticleSystem particles;
    WheelCollider wheel;
    CarController car;
    float speedDifference;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        wheel = GetComponentInParent<WheelCollider>();
        car = FindAnyObjectByType<CarController>();
        speedDifference = particles.main.startSpeed.constantMax - particles.main.startSpeed.constantMin;
    }

    private void Update()
    {
        if (car.Speed > 0 && wheel.isGrounded)
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
        var emission = particles.emission;
        var value = Mathf.Lerp(minParticles, maxParticles, car.Speed / car.SpeedLimit);
        emission.rateOverTime = value;
        var main = particles.main;
        var startSpeed = main.startSpeed;
        startSpeed.constantMin = Mathf.Lerp(minSpeed, maxSpeed, car.Speed / car.SpeedLimit);
        startSpeed.constantMax = Mathf.Lerp(minSpeed, maxSpeed, car.Speed / car.SpeedLimit) + speedDifference;
        main.startSpeed = startSpeed;
    }
}
