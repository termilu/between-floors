using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(menuName = "VRAR/Data/Anomaly Type", fileName = "AnomalyType")]
    public class AnomalyTypeSO : ScriptableObject
    {
        [Header("Identity")]
        public string id;

        [Header("Classification")] 
        public AnomalyCategory type = AnomalyCategory.Visual;
        
        [Header("Anomaly Intensity")]
        public float defaultIntensity = 1.0f;

        [Header("Prefab")] 
        public GameObject viewPrefab;

        [Header("Audio")] 
        public AudioClip defaultAudioClip;
        
        #if UNITY_EDITOR
            private void OnValidate()
            {
                if (string.IsNullOrWhiteSpace(id))
                    id = name.Trim();
            }
        #endif
    }
}