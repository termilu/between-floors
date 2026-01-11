using System.Collections.Generic;
using _Project.Scripts.Data;

namespace _Project.Scripts.Domain
{
    public sealed class GameSession
    {
        public int currentFloor { get; internal set; }
        public int failCount { get; internal set; }
        public int score { get; internal set; }
        public MovementType movementType { get; internal set; }
        
        public int highestFloorReached { get; internal set; } 
    }

    public sealed class FloorRuntime
    {
        public int floorId { get; }
        public List<AnomalyInstance> activeAnomalies { get; }

        public FloorRuntime(int floorId, List<AnomalyInstance> activeAnomalies)
        {
            this.floorId = floorId;
            this.activeAnomalies = activeAnomalies ?? new List<AnomalyInstance>();
        }

        public bool hasAnyActiveAnomalies()
        {
            for (int i = 0; i < activeAnomalies.Count; i++)
            {
                if (activeAnomalies[i].isActive)
                    return true;
            }
            
            return false;
        }
    }
    
    public sealed class AnomalyInstance
    {
        public string instanceId { get; }
        public bool isActive { get; internal set; }

        public AnomalyTypeSO anomalyType { get; }
        public float intensity { get; }

        public AnomalyInstance(
            string instanceId,
            bool isActive,
            AnomalyTypeSO anomalyType,
            float intensity)
        {
            this.instanceId = instanceId;
            this.isActive = isActive;
            this.anomalyType = anomalyType;
            this.intensity = intensity;
        }
    }
}