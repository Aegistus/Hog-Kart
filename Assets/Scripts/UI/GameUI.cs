using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private void Start()
    {
        RaceManager manager = FindAnyObjectByType<RaceManager>();
        if (manager != null)
        {
            manager.OnCountdownStart += () => ShowGameUI(true);
            ShowGameUI(false);
        }
    }

    public void ShowGameUI(bool show)
    {
        foreach (Transform transform in transform)
        {
            transform.gameObject.SetActive(show);
        }
    }
}
