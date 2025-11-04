using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoint : MonoBehaviour
{
    public event Action<Checkpoint> OnCheckpointReached;

    [SerializeField] GameObject checkpointVisuals;

    public bool CheckpointReached { get; private set; }
    public bool Activated { get; private set; }

    public Checkpoint Next { get; private set; }

    private void Awake()
    {
        checkpointVisuals.SetActive(false);
    }

    public void Initialize(Checkpoint next, Checkpoint previous, int checkpointNumber)
    {
        this.Next = next;
    }

    public void Activate()
    {
        checkpointVisuals.SetActive(true);
        Activated = true;
    }

    public void MarkAsReached()
    {
        CheckpointReached = true;
        checkpointVisuals.SetActive(false);
        if (Next != null)
        {
            Next.Activate();
            print(Next);
        }
        OnCheckpointReached?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckpointReached || !Activated)
        {
            return;
        }
        var player = other.GetComponentInParent<CarController>();
        if (player != null)
        {
            MarkAsReached();
        }
    }
}
