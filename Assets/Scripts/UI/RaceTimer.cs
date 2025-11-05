using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class RaceTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject checkpointTimerUIPrefab;

    public float CurrentTime => overallTime;

    float overallTime = 0f;
    bool started = false;

    private void Start()
    {
        CheckpointManager checkpointManager = FindAnyObjectByType<CheckpointManager>();
        checkpointManager.OnRaceEnd += () => started = false;
        for (int i = 0; i < checkpointManager.CheckpointCount; i++)
        {
            Instantiate(checkpointTimerUIPrefab, transform);
        }

        StartTimer();
    }

    public void StartTimer()
    {
        started = true;
        stringBuilder = new StringBuilder();
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
    StringBuilder stringBuilder;

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
