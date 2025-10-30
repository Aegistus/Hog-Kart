using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoint : MonoBehaviour
{
    public event Action<Checkpoint> OnCheckpointReached;

    [SerializeField] GameObject checkpointVisuals;

    public bool CheckpointReached { get; private set; }

    Checkpoint previous;
    Checkpoint next;
    int checkpointNumber;

    private void Awake()
    {
        checkpointVisuals.SetActive(false);
    }

    public void Initialize(Checkpoint next, Checkpoint previous, int checkpointNumber)
    {
        this.next = next;
        this.previous = previous;
        this.checkpointNumber = checkpointNumber;
    }

    public void Activate()
    {
        checkpointVisuals.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckpointReached)
        {
            return;
        }
        var player = other.GetComponentInParent<CarController>();
        if (player != null)
        {
            CheckpointReached = true;
            checkpointVisuals.SetActive(false);
            if (next != null)
            {
                next.Activate();
                print(next);
            }
            OnCheckpointReached?.Invoke(this);
        }
    }
}
