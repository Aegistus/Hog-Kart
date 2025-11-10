using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointTimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text checkpointNameText;
    [SerializeField] TMP_Text timeText;

    public Checkpoint LinkedCheckpoint { get; set; }

    public void Initialize(Checkpoint linked)
    {
        LinkedCheckpoint = linked;
        LinkedCheckpoint.OnCheckpointReached += UpdateCheckpointTime;
        checkpointNameText.text = LinkedCheckpoint.CheckpointName;
        var previousBest = SaveLoadSystem.Instance.GetCheckpointBestTime(RaceManager.Instance.MapName, LinkedCheckpoint.CheckpointIndex);
        timeText.text = RaceTimer.ConvertToTimeString(previousBest);
    }
    
    void UpdateCheckpointTime(Checkpoint _)
    {
        timeText.text = RaceTimer.ConvertToTimeString(RaceTimer.Instance.CurrentTime);
    }

    //private void Update()
    //{
    //    if (LinkedCheckpoint.Activated)
    //    {
    //        currentTimeText.text = RaceTimer.Instance.ConvertToTimeString(RaceTimer.Instance.CurrentTime);
    //    }
    //}
}
