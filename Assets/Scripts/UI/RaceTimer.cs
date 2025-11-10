using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class RaceTimer : MonoBehaviour
{
    public static RaceTimer Instance { get; private set; }

    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text previousBestText;
    [SerializeField] GameObject checkpointTimerUIPrefab;

    public float CurrentTime => overallTime;

    float overallTime = 0f;
    bool started = false;
    List<float> checkpointTimes = new();

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
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return null;
        RaceManager raceManager = RaceManager.Instance;
        raceManager.OnRaceEnd += EndTimer;
        previousBestText.text = "(PB: " + ConvertToTimeString(SaveLoadSystem.Instance.GetMapBestTime(raceManager.MapName)) + ")";
        // all checkpoints excluding finish line
        for (int i = 1; i < raceManager.CheckpointCount - 1; i++)
        {
            var checkpointTimer = Instantiate(checkpointTimerUIPrefab, transform).GetComponent<CheckpointTimerUI>();
            var checkpoint = raceManager.GetCheckpoint(i);
            checkpoint.OnCheckpointReached += (Checkpoint _) => checkpointTimes.Add(CurrentTime);
            checkpointTimer.Initialize(checkpoint);
        }

        RaceManager.Instance.OnRaceStart += StartTimer;
    }

    public void StartTimer()
    {
        started = true;
    }

    public void EndTimer()
    {
        started = false;
        float previousBestTime = SaveLoadSystem.Instance.GetMapBestTime(RaceManager.Instance.MapName);
        if (CurrentTime < previousBestTime || previousBestTime == 0)
        {
            SaveLoadSystem.Instance.SetMapBestTimes(RaceManager.Instance.MapName, CurrentTime, checkpointTimes.ToArray());
        }
        SaveLoadSystem.Instance.SaveData();
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
    static StringBuilder stringBuilder = new();

    public static string ConvertToTimeString(float time)
    {
        stringBuilder.Clear();
        if (time < 0)
        {
            stringBuilder.Append('-');
        }
        time = Mathf.Abs(time);
        // minutes
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

    public static string ConvertToSecondsString(float time, bool appendPlus = false)
    {
        stringBuilder.Clear();
        if (time < 0)
        {
            stringBuilder.Append('-');
        }
        else if (appendPlus)
        {
            stringBuilder.Append('+');
        }
        time = Mathf.Abs(time);
        // minutes

        // seconds
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
