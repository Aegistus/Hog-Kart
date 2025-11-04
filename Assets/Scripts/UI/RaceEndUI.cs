using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEndUI : MonoBehaviour
{
    private void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }


        FindAnyObjectByType<CheckpointManager>().OnRaceEnd += () =>
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        };
    }
}
