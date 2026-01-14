namespace _Project.Scripts.Domain.States
{
    public sealed class ExploreState : IGameState
    {
        private readonly GameStateMachine stateMachine;
        private readonly GameFlowService flow;

        public ExploreState(GameStateMachine stateMachine, GameFlowService flow)
        {
            this.stateMachine = stateMachine;
            this.flow = flow;
        }
        
        public void Enter() { }
        public void Exit() { }
        public void Update() { }

        public void PlayerEnteredElevator()
        {
            stateMachine.changeState(new DecideState(stateMachine, flow));
        }
    }
}