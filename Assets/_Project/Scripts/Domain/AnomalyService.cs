using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Data;

namespace _Project.Scripts.Domain
{
    public sealed class AnomalyService
    {
        private readonly Random anomalyRandom = new();

        public List<AnomalyInstance> genarateAnomalies(FloorConfigSO floorConfig)
        {
            var result = new List<AnomalyInstance>();
            
            if (floorConfig == null) return result;
            if (floorConfig.floorID == 0) return result;
            
            if(anomalyRandom.NextDouble() > floorConfig.baseAnomalyChance)  return result; //Entscheidet ob  Anomalie
            
            int count = Math.Clamp(floorConfig.maxAnomalies, 0, floorConfig.allowedAnomalies.Count);

            var pool = new List<AnomalyTypeSO>(floorConfig.allowedAnomalies);

            for (int i = 0; i < count && pool.Count > 0; i++)
            {
                int idx = anomalyRandom.Next(pool.Count);
                var anomalyType = pool[idx];
                pool.RemoveAt(idx);
                
                result.Add(new AnomalyInstance(
                    $"{floorConfig.floorID}_{anomalyType.id}_{Guid.NewGuid():N}",
                    true,
                    anomalyType,
                    anomalyType.defaultIntensity
                ));
            }
            
            return result;
        }
    }
}