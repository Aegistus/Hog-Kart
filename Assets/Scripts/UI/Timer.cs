using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;

    float time = 0f;
    bool started = false;

    private void Start()
    {
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
            time += Time.deltaTime;
            timerText.text = ConvertToTimeString(time);
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
