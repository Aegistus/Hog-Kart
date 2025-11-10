using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaceManager : MonoBehaviour
{
    public event Action OnRaceStart;
    public event Action OnRaceEnd;

    [SerializeField] string mapName;
    [SerializeField] float startRaceDelay = 5f;

    public static RaceManager Instance { get; private set; }
    public static readonly string[] allMapNames = { "TestScene", "Valhalla", };

    public string MapName => mapName;
    public Checkpoint CurrentRespawnCheckpoint { get; private set; }
    public Checkpoint NextCheckpoint => CurrentRespawnCheckpoint.Next;
    public int CheckpointCount => allCheckpoints.Count;

    List<Checkpoint> allCheckpoints = new();
    CarController car;

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
        car = FindAnyObjectByType<CarController>();
    }

    private void Start()
    {
        // initialize and get all checkpoints. Check points should be children of this gameobject ordered by their order in the race.
        allCheckpoints[0].Initialize(allCheckpoints[1], null, -1);
        for (int i = 1; i < allCheckpoints.Count - 1; i++)
        {
            allCheckpoints[i].Initialize(allCheckpoints[i + 1], allCheckpoints[i - 1], i - 1);
        }
        allCheckpoints[^1].Initialize(null, allCheckpoints[^2], allCheckpoints.Count - 2);

        foreach (var checkpoint in allCheckpoints)
        {
            checkpoint.OnCheckpointReached += OnCheckpointReached;
        }

        // mark the starting line as having been reached.
        allCheckpoints[0].MarkAsReached();

        car.SetCarFrozen(true);
        Invoke(nameof(StartRace), startRaceDelay);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
        }
    }

    public void StartRace()
    {
        car.SetCarFrozen(false);
        OnRaceStart?.Invoke();
    }

    public Checkpoint GetCheckpoint(int number)
    {
        return allCheckpoints[number];
    }

    public void ResetPlayer()
    {
        var player = FindObjectOfType<CarController>();
        var camera = FindObjectOfType<CameraController>();

        player.ResetToLastCheckpoint();
        player.transform.SetPositionAndRotation(CurrentRespawnCheckpoint.transform.position + Vector3.up * 3, CurrentRespawnCheckpoint.transform.rotation);
        camera.transform.position = CurrentRespawnCheckpoint.transform.position;
    }

    public void OnCheckpointReached(Checkpoint checkpoint)
    {
        CurrentRespawnCheckpoint = checkpoint;
        if (CurrentRespawnCheckpoint.Next == null)
        {
            Invoke(nameof(EndRace), .5f);
        }
    }

    void EndRace()
    {
        FindAnyObjectByType<CarController>().InputDisabled = true;
        OnRaceEnd?.Invoke();
    }
}
