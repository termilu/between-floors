using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Data;
using _Project.Scripts.Domain;

namespace _Project.Scripts.Presentation
{
    public sealed class AnomalyViewSpawner : MonoBehaviour
    {
        [Header("Optional Parent")]
        [SerializeField] private Transform parent;

        [Header("Optional Spawn Points")]
        [SerializeField] private List<Transform> spawnPoints = new();
        
        private readonly Dictionary<(int floorId, string typeId), AnomalyView> prewarmed = new();

        private GameContext context;
        private int nextSpawnIndex;

        public void Initialize(GameContext ctx, List<FloorConfigSO> floorConfigs)
        {
            if (context != null)
                Unsubscribe();

            context = ctx;

            if (context != null)
                Subscribe();

            PrewarmAll(floorConfigs);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            context.events.onAnomaliesActivated += OnAnomaliesActivated;
            context.events.onAnomaliesDeactivated += OnAnomaliesDeactivated;
        }

        private void Unsubscribe()
        {
            if (context == null) return;
            context.events.onAnomaliesActivated -= OnAnomaliesActivated;
            context.events.onAnomaliesDeactivated -= OnAnomaliesDeactivated;
        }

        private void PrewarmAll(List<FloorConfigSO> floorConfigs)
        {
            prewarmed.Clear();
            nextSpawnIndex = 0;

            if (floorConfigs == null) return;

            Transform p = parent != null ? parent : transform;

            for (int i = 0; i < floorConfigs.Count; i++)
            {
                var fc = floorConfigs[i];
                if (fc == null) continue;
                
                if (fc.floorID == 0) continue;

                if (fc.allowedAnomalies == null) continue;

                for (int j = 0; j < fc.allowedAnomalies.Count; j++)
                {
                    var type = fc.allowedAnomalies[j];
                    if (type == null) continue;
                    if (type.viewPrefab == null)
                    {
                        Debug.LogWarning($"[AnomalyViewSpawner] viewPrefab fehlt bei AnomalyTypeSO id='{type.id}'.");
                        continue;
                    }

                    var key = (fc.floorID, type.id);
                    if (prewarmed.ContainsKey(key))
                        continue;

                    Transform spawn = GetNextSpawnTransform();
                    Vector3 pos = spawn != null ? spawn.position : Vector3.zero;
                    Quaternion rot = spawn != null ? spawn.rotation : Quaternion.identity;

                    var go = Instantiate(type.viewPrefab, pos, rot, p);

                    if (!go.TryGetComponent<AnomalyView>(out var view))
                    {
                        view = go.AddComponent<AnomalyView>();
                    }
                    
                    view.Setup(new AnomalyInstance(
                        instanceId: $"{fc.floorID}_{type.id}_PREWARM",
                        isActive: false,
                        anomalyType: type,
                        intensity: type.defaultIntensity));

                    view.ApplyActiveState(false);

                    prewarmed[key] = view;
                }
            }

            Debug.Log($"[AnomalyViewSpawner] Prewarmed Views: {prewarmed.Count}");
        }

        private Transform GetNextSpawnTransform()
        {
            if (spawnPoints == null || spawnPoints.Count == 0) return null;

            var t = spawnPoints[nextSpawnIndex % spawnPoints.Count];
            nextSpawnIndex++;
            return t;
        }

        private void OnAnomaliesDeactivated(int floorId)
        {
            foreach (var kv in prewarmed)
            {
                if (kv.Key.floorId != floorId) continue;
                if (kv.Value != null)
                {
                    kv.Value.StopEffect();
                    kv.Value.ApplyActiveState(false);
                }
            }
        }

        private void OnAnomaliesActivated(int floorId, List<AnomalyInstance> anomalies)
        {
            OnAnomaliesDeactivated(floorId);
            
            if (anomalies == null) return;

            for (int i = 0; i < anomalies.Count; i++)
            {
                var inst = anomalies[i];
                if (inst == null || inst.anomalyType == null) continue;

                var key = (floorId, inst.anomalyType.id);

                if (prewarmed.TryGetValue(key, out var view) && view != null)
                {
                    view.Setup(inst);
                    view.ApplyActiveState(inst.isActive);
                    if (inst.isActive) view.PlayEffect();
                    else view.StopEffect();
                }
                else
                {
                    Debug.LogWarning($"[AnomalyViewSpawner] Kein prewarmed View gefunden f\u00fcr Floor {floorId}, Type '{inst.anomalyType.id}'.");
                }
            }
        }
    }
}
