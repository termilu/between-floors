namespace _Project.Scripts.Domain.States
{
    public sealed class MemorizeState : IGameState
    {
        private readonly GameStateMachine stateMachine;
        private readonly GameFlowService flow;

        public MemorizeState(GameStateMachine stateMachine, GameFlowService flow)
        {
            this.stateMachine = stateMachine;
            this.flow = flow;
        }

        public void Enter()
        {
            flow.StartFloor(0);
        }
        
        public void Exit() { }
        public void Update() { }

        public void PlayerReady()
        { 
            stateMachine.changeState(new ExploreState(stateMachine, flow));
        }
    }
}