using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    [SerializeField] AudioSource engineAudio;
    [SerializeField] float maxPitch = .5f;
    [SerializeField] float maxVolume = 2f;
    [SerializeField] float audioChangeSpeed = 1f;

    CarController car;

    private void Awake()
    {
        car = GetComponent<CarController>();
        car.OnReset += () =>
        {
            engineAudio.pitch = 1;
            engineAudio.volume = 1;
        };
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, 1 + ((car.Speed / car.SpeedLimit) * maxPitch), audioChangeSpeed * Time.deltaTime);
            engineAudio.volume = Mathf.Lerp(engineAudio.volume, 1 + ((car.Speed / car.SpeedLimit) * maxVolume), audioChangeSpeed * Time.deltaTime);
        }
        else
        {
            engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, 1, audioChangeSpeed * Time.deltaTime);
            engineAudio.volume = Mathf.Lerp(engineAudio.volume, 1, audioChangeSpeed * Time.deltaTime);
        }
    }
}
