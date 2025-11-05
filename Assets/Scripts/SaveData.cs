using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData : MonoBehaviour
{
    public class MapPerformanceData
    {
        public string mapName;
        public float bestOverallTime;
        public float[] checkpointTimes;
    }

    public List<MapPerformanceData> mapData = new();
}
