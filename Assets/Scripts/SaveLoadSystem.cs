using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using EditorAttributes;
using System;

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

    public void SaveData()
    {
        try
        {
            FileStream saveStream = new (filePath, FileMode.OpenOrCreate);
            BinaryFormatter converter = new();
            converter.Serialize(saveStream, currentSaveData);
            saveStream.Close();
        }
        catch (Exception e)
        {
            print(e.ToString());
            Debug.LogWarning("WARNING: Unable to save game data to file.");
        }
    }

    public void LoadData()
    {
        SaveData saveData;
        try
        {
            FileStream loadStream = new(filePath, FileMode.Open);
            BinaryFormatter converter = new();
            saveData = converter.Deserialize(loadStream) as SaveData;
            loadStream.Close();
        }
        catch (Exception e)
        {
            print(e.ToString());
            Debug.LogWarning("No save file found, generating default save data");
            saveData = new();
            saveData.InitializeDefault(RaceManager.allMapNames);
        }
        currentSaveData = saveData;
    }

    [GUIColor(GUIColor.Red)]
    [Button("CLEAR DATA", 30f)]
    public void ClearData()
    {
        filePath = "C:/Users/joeba/AppData/LocalLow/DefaultCompany/Drifting Car Test/data";
        currentSaveData = new();
        currentSaveData.InitializeDefault(RaceManager.allMapNames);
        SaveData();
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

    public void SetMapBestTimes(string mapName, float newTime, float[] checkpointTimes)
    {
        for (int i = 0; i < currentSaveData.mapData.Count; i++)
        {
            if (currentSaveData.mapData[i].mapName == mapName)
            {
                currentSaveData.mapData[i].bestOverallTime = newTime;
                for (int j = 0; j < checkpointTimes.Length; j++)
                {
                    currentSaveData.mapData[i].checkpointTimes[j] = checkpointTimes[j];
                }
                return;
            }
        }
    }
}
