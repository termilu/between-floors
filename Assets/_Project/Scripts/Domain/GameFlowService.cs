using System.Collections.Generic;
using _Project.Scripts.Data;

namespace _Project.Scripts.Domain
{
    public sealed class GameFlowService
    {
        private readonly GameSettingsSO settings;
        private readonly Dictionary<int, FloorConfigSO> floorConfigs;
        private readonly AnomalyService anomalies;
        private readonly GameEventsHub events;
        
        public GameSession session { get; }
        public FloorRuntime currentFloorRuntime { get; private set; }

        public GameFlowService(
            GameSettingsSO settings,
            IEnumerable<FloorConfigSO> floorConfigs,
            AnomalyService anomalies,
            GameEventsHub events)
        {
            this.settings = settings;
            this.anomalies = anomalies;
            this.events = events;

            this.floorConfigs = new Dictionary<int, FloorConfigSO>();
            
            foreach (var f in floorConfigs)
                this.floorConfigs[f.floorID] = f;
            session = new GameSession();
        }

        public void StartNewSession()
        {
            session.score = 0;
            session.failCount = 0;
            session.currentFloor = 0;
            session.highestFloorReached = 0;
            session.movementType = settings.defaultMovementType;

            StartFloor(0);
            events.RaiseScoreUpdated(session.score);
        }

        public void StartFloor(int floorId)
        {
            session.currentFloor = floorId;
            
            if(floorId > session.highestFloorReached)
                session.highestFloorReached = floorId;

            var config = floorConfigs.ContainsKey(floorId)
                ? floorConfigs[floorId]
                : floorConfigs[0];
            
            var generatedAnomalies = anomalies.genarateAnomalies(config);
            
            currentFloorRuntime = new FloorRuntime(floorId, generatedAnomalies);
            
            events.RaiseFloorChanged(currentFloorRuntime);
            events.RaiseAnomaliesGenerated(generatedAnomalies);
        }

        public void ProcessDecision(AnomalyReportData report)
        {
            bool hadAnomaly = currentFloorRuntime.hasAnyActiveAnomalies();
            bool correctDecision = hadAnomaly == report.reportedAnomaly;
            int nextFloor;

            if (correctDecision)
            {
                session.score++;
                events.RaiseScoreUpdated(session.score);
                nextFloor = session.currentFloor + 1;
            }
            else
            {
                session.failCount++;
                StartFloor(0);
                nextFloor = 0;

                if (session.failCount >= settings.maxFails)
                {
                    events.RaiseGameOver(new GameOverData(
                        session.score,
                        session.highestFloorReached,
                        session.failCount));
                }
            }
            
            events.RaiseDecisionOutcome(new DecisionOutcomeData(
                wasCorrect: correctDecision,
                hadAnomaly: hadAnomaly,
                newScore: session.score,
                newFailCount: session.failCount,
                nextFloor: nextFloor));
                
        }
    }
}