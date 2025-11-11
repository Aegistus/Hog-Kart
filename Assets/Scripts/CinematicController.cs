using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CinematicController : MonoBehaviour
{
    [SerializeField] float timeToSkipTo = 60f;
    
    PlayableDirector director;
    bool skipped = false;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !skipped)
        {
            director.time = timeToSkipTo;
            skipped = true;
        }
    }
}
