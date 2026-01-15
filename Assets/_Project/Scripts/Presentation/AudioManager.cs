using UnityEngine;
using _Project.Scripts.Domain;

namespace _Project.Scripts.Presentation
{
    public sealed class AudioManager : MonoBehaviour
    {
        [Header("Optional AudioSource")]
        [SerializeField] private AudioSource sfxSource;

        private GameContext context;

        public void Initialize(GameContext ctx)
        {
            if (context != null)
                Unsubscribe();

            context = ctx;

            if (context != null)
                Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            context.events.onDecisionOutcome += OnDecisionOutcome;
            context.events.onGameOver += OnGameOver;
        }

        private void Unsubscribe()
        {
            if (context == null) return;

            context.events.onDecisionOutcome -= OnDecisionOutcome;
            context.events.onGameOver -= OnGameOver;
        }

        private void OnDecisionOutcome(DecisionOutcomeData data)
        {
            if (sfxSource == null) return;
            
           //TODO
        }

        private void OnGameOver(GameOverData data)
        {
            //TODO
        }

        
        public void PlaySfx(string eventId)
        {
            //TODO
        }
    }
}