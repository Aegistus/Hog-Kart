using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class RaceTimer : MonoBehaviour
{
    public static RaceTimer Instance { get; private set; }

    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject checkpointTimerUIPrefab;

    public float CurrentTime => overallTime;

    float overallTime = 0f;
    bool started = false;

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
        RaceManager raceManager = RaceManager.Instance;
        raceManager.OnRaceEnd += () => started = false;
        // all checkpoints excluding finish line
        for (int i = 1; i < raceManager.CheckpointCount - 1; i++)
        {
            var checkpointTimer = Instantiate(checkpointTimerUIPrefab, transform).GetComponent<CheckpointTimerUI>();
            var checkpoint = raceManager.GetCheckpoint(i);
            checkpointTimer.LinkedCheckpoint = checkpoint;
            checkpointTimer.checkpointNameText.text = checkpoint.CheckpointName;
            checkpointTimer.previousTimeText.text = ConvertToTimeString(SaveLoadSystem.Instance.GetCheckpointBestTime(raceManager.MapName, i));
        }

        StartTimer();
    }

    public void StartTimer()
    {
        started = true;
    }

    private void Update()
    {
        if (started)
        {
            overallTime += Time.deltaTime;
            timerText.text = ConvertToTimeString(overallTime);

        }
    }

    //string format = "00";
    StringBuilder stringBuilder = new();

    public string ConvertToTimeString(float time)
    {
        // minutes
        stringBuilder.Clear();
        int minutes = (int)time / 60;
        if (minutes < 10)
        {
            stringBuilder.Append('0');
        }
        stringBuilder.Append(minutes);
        //stringBuilder.Append(((int)(time / 60)).ToString(format));

        // seconds
        stringBuilder.Append(":");
        int seconds = (int)time % 60;
        if (seconds < 10)
        {
            stringBuilder.Append('0');
        }
        stringBuilder.Append(seconds);

        // milliseconds
        stringBuilder.Append(".");
        int milliseconds = (int)(time * 100) % 100;
        if (milliseconds < 10)
        {
            stringBuilder.Append('0');
        }
        stringBuilder.Append(milliseconds);
        return stringBuilder.ToString();
    }
}
