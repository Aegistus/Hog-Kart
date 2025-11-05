using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public class MapPerformanceData
    {
        public string mapName;
        public float bestOverallTime;
        public float[] checkpointTimes;
    }

    public List<MapPerformanceData> mapData = new();

    public void InitializeDefault(string[] mapNames)
    {
        for (int i = 0; i < mapNames.Length; i++)
        {
            MapPerformanceData map = new();
            map.mapName = mapNames[i];
            map.bestOverallTime = float.MaxValue;
            map.checkpointTimes = new float[100];
        }
    }
}
