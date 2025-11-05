using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointTimerUI : MonoBehaviour
{
    public TMP_Text checkpointNameText;
    public TMP_Text currentTimeText;
    public TMP_Text previousTimeText;

    public Checkpoint LinkedCheckpoint { get; set; }

    private void Update()
    {
        if (LinkedCheckpoint.Activated)
        {
            currentTimeText.text = RaceTimer.Instance.ConvertToTimeString(RaceTimer.Instance.CurrentTime);
        }
    }
}
