using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointManager : MonoBehaviour
{
    public event Action OnRaceEnd;

    public CheckpointManager Instance { get; private set; }
    public Checkpoint CurrentRespawnCheckpoint { get; private set; }

    List<Checkpoint> allCheckpoints = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        // initialize and get all checkpoints. Check points should be children ordered by their order in the race.
        allCheckpoints.AddRange(GetComponentsInChildren<Checkpoint>());
        allCheckpoints[0].Initialize(allCheckpoints[1], null, 1);
        for (int i = 1; i < allCheckpoints.Count - 1; i++)
        {
            allCheckpoints[i].Initialize(allCheckpoints[i + 1], allCheckpoints[i - 1], i + 1);
        }
        allCheckpoints[^1].Initialize(null, allCheckpoints[^2], allCheckpoints.Count);

        foreach (var checkpoint in allCheckpoints)
        {
            checkpoint.OnCheckpointReached += OnCheckpointReached;
        }

        // mark the starting line as having been reached.
        allCheckpoints[0].MarkAsReached();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
        }
    }

    public void ResetPlayer()
    {
        var player = FindObjectOfType<CarController>();
        var camera = FindObjectOfType<CameraController>();

        player.Reset();
        player.transform.position = CurrentRespawnCheckpoint.transform.position + Vector3.up * 3;
        player.transform.rotation = CurrentRespawnCheckpoint.transform.rotation;
        camera.transform.position = CurrentRespawnCheckpoint.transform.position;
    }

    public void OnCheckpointReached(Checkpoint checkpoint)
    {
        CurrentRespawnCheckpoint = checkpoint;
        if (CurrentRespawnCheckpoint.Next == null)
        {
            // End Race
            OnRaceEnd?.Invoke();
        }
    }
}
