using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem Instance { get; private set; }

    SaveData currentSaveData;
    string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/data";
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        LoadData();
    }

    public void SaveData(string map, float bestTime, float[] checkpointTimes)
    {

    }

    public void LoadData()
    {
        SaveData saveData;
        try
        {
            FileStream saveStream = new FileStream(filePath, FileMode.Open);
            BinaryFormatter converter = new();
            saveData = converter.Deserialize(saveStream) as SaveData;
            saveStream.Close();
        }
        catch
        {
            Debug.LogWarning("No save file found, generating default save data");
            saveData = new();
            saveData.InitializeDefault(RaceManager.allMapNames);
        }
        currentSaveData = saveData;
    }

    public float GetMapBestTime(string mapName)
    {
        for (int i = 0; i < currentSaveData.mapData.Count; i++)
        {
            if (currentSaveData.mapData[i].mapName == mapName)
            {
                return currentSaveData.mapData[i].bestOverallTime;
            }
        }
        return 0;
    }

    public float GetCheckpointBestTime(string mapName, int checkpointIndex)
    {
        for (int i = 0; i < currentSaveData.mapData.Count; i++)
        {
            if (currentSaveData.mapData[i].mapName == mapName)
            {
                return currentSaveData.mapData[i].checkpointTimes[checkpointIndex];
            }
        }
        return 0;
    }
}
