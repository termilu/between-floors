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
        public float resultDurationSeconds => settings != null ? settings.resultDurationSeconds : 2.0f;

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
            if (currentFloorRuntime != null)
                events.RaiseAnomaliesDeactivated(currentFloorRuntime.floorId);

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
            events.RaiseAnomaliesActivated(floorId, generatedAnomalies);
        }

        public DecisionResult ProcessDecision(AnomalyReportData report)
        {
            bool hadAnomaly = currentFloorRuntime.hasAnyActiveAnomalies();
            bool correctDecision = hadAnomaly == report.reportedAnomaly;

            int nextFloor = session.currentFloor;
            bool isGameOver = false;

            if (correctDecision)
            {
                session.score++;
                events.RaiseScoreUpdated(session.score);

                nextFloor = session.currentFloor + 1;

                events.RaiseDecisionOutcome(new DecisionOutcomeData(
                    wasCorrect: true,
                    hadAnomaly: hadAnomaly,
                    newScore: session.score,
                    newFailCount: session.failCount,
                    nextFloor: nextFloor));

                return new DecisionResult(nextFloor, isGameOver);
            }

            // falsche Entscheidung
            session.failCount++;

            int maxFails = settings != null ? settings.maxFails : 3;

            // GameOver
            if (session.failCount >= maxFails)
            {
                isGameOver = true;

                events.RaiseDecisionOutcome(new DecisionOutcomeData(
                    wasCorrect: false,
                    hadAnomaly: hadAnomaly,
                    newScore: session.score,
                    newFailCount: session.failCount,
                    nextFloor: 0));

                events.RaiseGameOver(new GameOverData(
                    session.score,
                    session.highestFloorReached,
                    session.failCount));

                return new DecisionResult(0, isGameOver);
            }

            // Kein GameOver -> zurück auf Floor 0
            nextFloor = 0;

            events.RaiseDecisionOutcome(new DecisionOutcomeData(
                wasCorrect: false,
                hadAnomaly: hadAnomaly,
                newScore: session.score,
                newFailCount: session.failCount,
                nextFloor: nextFloor));

            return new DecisionResult(nextFloor, isGameOver);
        }


        public void SetMovementType(MovementType type)
        {
            session.movementType = type;
        }
    }
    
    public readonly struct DecisionResult
    {
        public readonly int nextFloor;
        public readonly bool isGameOver;

        public DecisionResult(int nextFloor, bool isGameOver)
        {
            this.nextFloor = nextFloor;
            this.isGameOver = isGameOver;
        }
    }

}


