using System;
using System.Collections.Generic;

namespace _Project.Scripts.Domain
{
    public sealed class GameEventsHub
    {
        public event Action<FloorRuntime> onFloorChanged;
        public event Action<List<AnomalyInstance>> onAnomaliesGenerated;
        public event Action<int> onAnomaliesDeactivated;
        public event Action<int, List<AnomalyInstance>> onAnomaliesActivated;
        public event Action<IGameState> onStateChanged;
        public event Action<GameOverData> onGameOver;
        public event Action<int> onScoreUpdated;

        public event Action<DecisionOutcomeData> onDecisionOutcome;

        internal void RaiseFloorChanged(FloorRuntime floor)
            => onFloorChanged?.Invoke(floor);

        internal void RaiseAnomaliesGenerated(List<AnomalyInstance> anomalies)
            => onAnomaliesGenerated?.Invoke(anomalies);

        internal void RaiseAnomaliesDeactivated(int floorId)
            => onAnomaliesDeactivated?.Invoke(floorId);

        internal void RaiseAnomaliesActivated(int floorId, List<AnomalyInstance> anomalies)
            => onAnomaliesActivated?.Invoke(floorId, anomalies);

        internal void RaiseStateChanged(IGameState state)
            => onStateChanged?.Invoke(state);

        internal void RaiseGameOver(GameOverData data)
            => onGameOver?.Invoke(data);

        internal void RaiseScoreUpdated(int score)
            => onScoreUpdated?.Invoke(score);

        internal void RaiseDecisionOutcome(DecisionOutcomeData data)
            => onDecisionOutcome?.Invoke(data);
    }
}
