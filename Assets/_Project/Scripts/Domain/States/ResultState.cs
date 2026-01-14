namespace _Project.Scripts.Domain.States
{
    public sealed class ResultState : IGameState
    {
        private readonly GameStateMachine stateMachine;
        private readonly GameFlowService flow;
        
        private float remainingSeconds;

        public ResultState(GameStateMachine stateMachine, GameFlowService flow)
        {
            this.stateMachine = stateMachine;
            this.flow = flow;
        }

        public void Enter()
        {
            remainingSeconds = 2.0f;
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
        }
    }
}