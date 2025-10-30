using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    List<Checkpoint> allCheckpoints = new();

    private void Start()
    {
        // initialize all checkpoints
        allCheckpoints.AddRange(GetComponentsInChildren<Checkpoint>());
        allCheckpoints[0].Initialize(allCheckpoints[1], null, 1);
        for (int i = 1; i < allCheckpoints.Count - 1; i++)
        {
            allCheckpoints[i].Initialize(allCheckpoints[i + 1], allCheckpoints[i - 1], i + 1);
        }
        allCheckpoints[allCheckpoints.Count - 1].Initialize(null, allCheckpoints[allCheckpoints.Count - 2], allCheckpoints.Count);

        allCheckpoints[0].Activate();
    }

}
