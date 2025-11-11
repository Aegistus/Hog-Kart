using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private void Start()
    {
        ShowGameUI(false);
        FindAnyObjectByType<RaceManager>().OnCountdownStart += () => ShowGameUI(true);
    }

    public void ShowGameUI(bool show)
    {
        foreach (Transform transform in transform)
        {
            transform.gameObject.SetActive(show);
        }
    }
}
