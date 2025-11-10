using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalTimeUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    RaceTimer timer;

    public void Start()
    {
        timer = FindAnyObjectByType<RaceTimer>();
        FindAnyObjectByType<RaceManager>().OnRaceEnd += UpdateFinalTime;
    }

    private void UpdateFinalTime()
    {
        text.text = RaceTimer.ConvertToTimeString(timer.CurrentTime);
    }
}
