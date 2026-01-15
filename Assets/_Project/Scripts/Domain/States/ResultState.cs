namespace _Project.Scripts.Domain.States
{
    public sealed class ResultState : IGameState
    {
        private readonly GameStateMachine stateMachine;
        private readonly GameFlowService flow;
        
        private readonly int nextFloor;
        private readonly bool isGameOver;
        
        private float remainingSeconds;

        public ResultState(GameStateMachine stateMachine, GameFlowService flow, float durationSeconds, int nextFloor,
            bool isGameOver)
        {
            this.stateMachine = stateMachine;
            this.flow = flow;
            this.nextFloor = nextFloor;
            this.isGameOver = isGameOver;

            remainingSeconds = durationSeconds;
        }

        public void Enter()
        {
        }
        
        public void Exit() { }
        public void Update() { }

        public void Tick(float deltaTime)
        {
            remainingSeconds -= deltaTime;
            if (remainingSeconds <= 0.0f)
            {
                stateMachine.changeState(new ExploreState(stateMachine, flow));
            }
            
            flow.StartFloor(nextFloor);
            stateMachine.changeState(new ExploreState(stateMachine, flow));
        }
    }
}
