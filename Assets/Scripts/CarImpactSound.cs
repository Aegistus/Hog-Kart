using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarImpactSound : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Playing");
        audioSource.Play();
    }
}
