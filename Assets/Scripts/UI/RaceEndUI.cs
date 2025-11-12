using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RaceEndUI : MonoBehaviour
{
    [SerializeField] Color newRecordColor;
    [SerializeField] GameObject menu;
    [SerializeField] TMP_Text finalTimeText;
    [SerializeField] TMP_Text previousBestText;
    [SerializeField] GameObject newRecordElements;
    RaceTimer timer;
    float previousBest;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        timer = FindAnyObjectByType<RaceTimer>(FindObjectsInactive.Include);
        FindAnyObjectByType<RaceManager>().OnRaceEnd += ShowMenu;

        previousBest = SaveLoadSystem.Instance.GetMapBestTime(RaceManager.Instance.MapName);
        if (previousBest < float.MaxValue)
        {
            previousBestText.text += RaceTimer.ConvertToTimeString(previousBest);
        }
    }

    void ShowMenu()
    {
        menu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        finalTimeText.text = RaceTimer.ConvertToTimeString(timer.CurrentTime);
        if (timer.CurrentTime >= previousBest)
        {
            newRecordElements.SetActive(false);
        }
        else
        {
            finalTimeText.color = newRecordColor;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
