using UnityEngine;
using _Project.Scripts.Domain.States;

namespace _Project.Scripts.Presentation
{
    public sealed class ElevatorController : MonoBehaviour
    {
        private GameContext context;

        public void Initialize(GameContext context)
        {
            this.context = context;
        }

        public void OnPlayerEnteredElevator()
        {
            if (context == null) return;

            if (context.stateMachine.currentState is ExploreState exploreState)
            {
                exploreState.PlayerEnteredElevator();
            }
        }

        public void OnButtonReportAnomaly()
        {
            SubmitDecision(true);
        }

        public void OnButtonNoAnomaly()
        {
            SubmitDecision(false);
        }

        private void SubmitDecision(bool reportedAnomaly)
        {
            if (context == null) return;

            if (context.stateMachine.currentState is DecideState decideState)
            {
                decideState.SubmitDecision(reportedAnomaly);
            }
        }
    }
}