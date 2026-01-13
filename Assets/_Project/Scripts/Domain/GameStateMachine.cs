namespace _Project.Scripts.Domain
{
    public sealed class GameStateMachine
    {
        private readonly GameEventsHub events;
        
        public IGameState currentState { get; private set; }

        public GameStateMachine(GameEventsHub eventsHub)
        {
            events = eventsHub;
        }

        public void changeState(IGameState newState)
        {
            if (newState == null) return;
            
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
            
            events.RaiseStateChanged(currentState);
        }

        public void Update()
        {
            currentState?.Update();
        }
    }

    public interface IGameState
    {
        void Enter();
        void Exit();
        void Update();
    }
}