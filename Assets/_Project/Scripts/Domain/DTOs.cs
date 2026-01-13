namespace _Project.Scripts.Domain
{
    public readonly struct AnomalyReportData
    {
        public readonly bool reportedAnomaly;

        public AnomalyReportData(bool reportedAnomaly)
        {
            this.reportedAnomaly = reportedAnomaly;
        }
    }
    
    public readonly struct DecisionOutcomeData
    {
        public readonly bool wasCorrect;
        public readonly bool hadAnomaly;
        public readonly int newScore;
        public readonly int newFailCount;
        public readonly int nextFloor;

        public DecisionOutcomeData(
            bool wasCorrect,
            bool hadAnomaly,
            int newScore,
            int newFailCount,
            int nextFloor)
        {
            this.wasCorrect = wasCorrect;
            this.hadAnomaly = hadAnomaly;
            this.newScore = newScore;
            this.newFailCount = newFailCount;
            this.nextFloor = nextFloor;
        }
    }

    public readonly struct GameOverData
    {
        public readonly int finalScore;
        public readonly int highestFloorReached;
        public readonly int failCount;

        public GameOverData(int finalScore, int highestFloorReached, int failCount)
        {
            this.finalScore = finalScore;
            this.highestFloorReached = highestFloorReached;
            this.failCount = failCount;
        }
    }
}