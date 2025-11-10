using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoint : MonoBehaviour
{
    public event Action<Checkpoint> OnCheckpointReached;

    [SerializeField] GameObject checkpointVisuals;
    [SerializeField] string checkpointName = "Checkpoint";

    public bool CheckpointReached { get; private set; }
    public bool Activated { get; private set; }

    public string CheckpointName => checkpointName;
    public int CheckpointIndex { get; private set; }
    public Checkpoint Next { get; private set; }
    public Checkpoint Previous { get; private set; }

    AudioSource reachedAudio;

    private void Awake()
    {
        checkpointVisuals.SetActive(false);
        reachedAudio = GetComponent<AudioSource>();
    }

    public void Initialize(Checkpoint next, Checkpoint previous, int checkpointIndex)
    {
        Next = next;
        CheckpointIndex = checkpointIndex;
    }

    public void Activate()
    {
        checkpointVisuals.SetActive(true);
        Activated = true;
    }

    public void MarkAsReached()
    {
        CheckpointReached = true;
        Activated = false;
        checkpointVisuals.SetActive(false);
        if (Next != null)
        {
            Next.Activate();
            print(Next);
        }
        if (reachedAudio)
        {
            reachedAudio.Play();
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
