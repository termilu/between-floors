using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(menuName = "VRAR/Data/Floor Config", fileName = "FloorConfig")]
    public class FloorConfigSO : ScriptableObject
    {
        [Header("Floor")]
        public int floorID;

        [Header("Anomalies")] 
        public List<AnomalyTypeSO> allowedAnomalies = new();

        [Header("Anomalies per floor")] 
        public int maxAnomalies = 1;
        
        [Header("Chance for Anomaly")]
        public float baseAnomalyChance = 0.4f;
        
        #if UNITY_EDITOR
            private void OnValidate()
            {
                if (floorID == 0)
                {
                    baseAnomalyChance = 0f;
                    maxAnomalies = 0;
                }
            }
        #endif
    }
}