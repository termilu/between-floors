using UnityEngine;
using _Project.Scripts.Domain;

namespace _Project.Scripts.Presentation
{
    public sealed class HUDController : MonoBehaviour
    {
        [Header("Optional UI Bindings")]
        [SerializeField] private TMPro.TMP_Text floorText;
        [SerializeField] private TMPro.TMP_Text stateText;
        [SerializeField] private TMPro.TMP_Text scoreText;

        private GameContext context;

        public void Initialize(GameContext ctx)
        {
            if (context != null)
                Unsubscribe();

            context = ctx;

            if (context != null)
                Subscribe();
            
            if (context != null)
            {
                ShowScore(context.flow.session.score);
                ShowFloor(context.flow.session.currentFloor);
                ShowState(context.stateMachine.currentState != null ? context.stateMachine.currentState.GetType().Name : "-");
            }
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            context.events.onFloorChanged += OnFloorChanged;
            context.events.onStateChanged += OnStateChanged;
            context.events.onScoreUpdated += OnScoreUpdated;
        }

        private void Unsubscribe()
        {
            if (context == null) return;

            context.events.onFloorChanged -= OnFloorChanged;
            context.events.onStateChanged -= OnStateChanged;
            context.events.onScoreUpdated -= OnScoreUpdated;
        }

        private void OnFloorChanged(FloorRuntime floor)
        {
            ShowFloor(floor != null ? floor.floorId : 0);
        }

        private void OnStateChanged(IGameState state)
        {
            ShowState(state != null ? state.GetType().Name : "-");
        }

        private void OnScoreUpdated(int score)
        {
            ShowScore(score);
        }

        public void ShowFloor(int floor)
        {
            if (floorText != null) floorText.text = $"Floor: {floor}";
        }

        public void ShowState(string stateName)
        {
            if (stateText != null) stateText.text = $"State: {stateName}";
        }

        public void ShowScore(int score)
        {
            if (scoreText != null) scoreText.text = $"Score: {score}";
        }
    }
}
