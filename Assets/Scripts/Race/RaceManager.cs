using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaceManager : MonoBehaviour
{
    public event Action OnRaceEnd;

    [SerializeField] string mapName;

    public static RaceManager Instance { get; private set; }
    public static readonly string[] allMapNames = { "TestScene", "Valhalla", };

    public string MapName => mapName;
    public Checkpoint CurrentRespawnCheckpoint { get; private set; }
    public Checkpoint NextCheckpoint => CurrentRespawnCheckpoint.Next;
    public int CheckpointCount => allCheckpoints.Count;

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
        allCheckpoints.AddRange(GetComponentsInChildren<Checkpoint>());
    }

    private void Start()
    {
        // initialize and get all checkpoints. Check points should be children ordered by their order in the race.
        allCheckpoints[0].Initialize(allCheckpoints[1], null, 1);
        for (int i = 1; i < allCheckpoints.Count - 1; i++)
        {
            allCheckpoints[i].Initialize(allCheckpoints[i + 1], allCheckpoints[i - 1], i);
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

    public Checkpoint GetCheckpoint(int number)
    {
        return allCheckpoints[number];
    }

    public void ResetPlayer()
    {
        var player = FindObjectOfType<CarController>();
        var camera = FindObjectOfType<CameraController>();

        player.Reset();
        player.transform.SetPositionAndRotation(CurrentRespawnCheckpoint.transform.position + Vector3.up * 3, CurrentRespawnCheckpoint.transform.rotation);
        camera.transform.position = CurrentRespawnCheckpoint.transform.position;
    }

    public void OnCheckpointReached(Checkpoint checkpoint)
    {
        CurrentRespawnCheckpoint = checkpoint;
        if (CurrentRespawnCheckpoint.Next == null)
        {
            EndRace();
        }
    }

    void EndRace()
    {
        FindAnyObjectByType<CarController>().InputDisabled = true;
        OnRaceEnd?.Invoke();
    }
}
