using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPad : MonoBehaviour
{
    [SerializeField] float boostForce;

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponentInParent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(boostForce * transform.forward);
        }
    }
}
