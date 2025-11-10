using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointTimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text checkpointNameText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text deltaText;

    float previousBest;
    public Checkpoint LinkedCheckpoint { get; set; }

    public void Initialize(Checkpoint linked)
    {
        LinkedCheckpoint = linked;
        LinkedCheckpoint.OnCheckpointReached += UpdateCheckpointTime;
        checkpointNameText.text = LinkedCheckpoint.CheckpointName;
        previousBest = SaveLoadSystem.Instance.GetCheckpointBestTime(RaceManager.Instance.MapName, LinkedCheckpoint.CheckpointIndex);
        timeText.text = RaceTimer.ConvertToTimeString(previousBest);
        deltaText.text = "";
    }
    
    void UpdateCheckpointTime(Checkpoint _)
    {
        timeText.text = RaceTimer.ConvertToTimeString(RaceTimer.Instance.CurrentTime);
    }

    private void Update()
    {
        if (LinkedCheckpoint.Activated)
        {
            var delta = RaceTimer.Instance.CurrentTime - previousBest;
            deltaText.text = RaceTimer.ConvertToSecondsString(delta, true);
            if (delta > 0)
            {
                deltaText.color = Color.red;
            }
        }
    }
}
