using _Project.Scripts.Domain;

namespace _Project.Scripts.Domain.States
{
    public sealed class MainMenuState : IGameState
    {
        private readonly GameStateMachine stateMachine;

        public MainMenuState(GameStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        
        public void Enter() { }
        public void Exit() { }
        public void Update() { }

        public void StartGame(GameFlowService flow)
        {
            if (flow == null) return;
            
            flow.StartNewSession();
            stateMachine.changeState(new MemorizeState(stateMachine, flow));
        }
    }
}