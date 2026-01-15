namespace _Project.Scripts.Domain.States
{
    public sealed class DecideState : IGameState
    {
        private readonly GameStateMachine stateMachine;
        private readonly GameFlowService flow;

        public DecideState(GameStateMachine stateMachine, GameFlowService flow)
        {
            this.stateMachine = stateMachine;
            this.flow = flow;
        }
        
        public void Enter() { }
        public void Exit() { }
        public void Update() { }

        public void SubmitDecision(bool reportedAnomaly)
        {
            var result = flow.ProcessDecision(new AnomalyReportData(reportedAnomaly));

            stateMachine.changeState(new ResultState(
                stateMachine,
                flow,
                durationSeconds: flow.resultDurationSeconds,
                nextFloor: result.nextFloor,
                isGameOver: result.isGameOver));
        }
    }
}
