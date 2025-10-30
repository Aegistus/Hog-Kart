using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public CheckpointManager Instance { get; private set; }
    public Checkpoint CurrentCheckpoint { get; private set; }

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
        // initialize all checkpoints
        allCheckpoints.AddRange(GetComponentsInChildren<Checkpoint>());
        allCheckpoints[0].Initialize(allCheckpoints[1], null, 1);
        for (int i = 1; i < allCheckpoints.Count - 1; i++)
        {
            allCheckpoints[i].Initialize(allCheckpoints[i + 1], allCheckpoints[i - 1], i + 1);
        }
        allCheckpoints[^1].Initialize(null, allCheckpoints[^2], allCheckpoints.Count);

        foreach (var checkpoint in allCheckpoints)
        {
            checkpoint.OnCheckpointReached += (checkpoint) => CurrentCheckpoint = checkpoint;
        }

        allCheckpoints[0].Activate();
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
        player.transform.position = CurrentCheckpoint.transform.position + Vector3.up * 3;
        player.transform.rotation = CurrentCheckpoint.transform.rotation;
        camera.transform.position = CurrentCheckpoint.transform.position;
    }
}
