using _Project.Scripts.Domain;

namespace _Project.Scripts.Presentation
{
    public sealed class GameContext
    {
        public GameEventsHub events { get; }
        public GameStateMachine stateMachine { get; }
        public GameFlowService flow { get; }
        public AnomalyService anomalies { get; }

        public GameContext(
            GameEventsHub events,
            GameStateMachine stateMachine,
            GameFlowService flow,
            AnomalyService anomalies)
        {
            this.events = events;
            this.stateMachine = stateMachine;
            this.flow = flow;
            this.anomalies = anomalies;
        }
    }
}   