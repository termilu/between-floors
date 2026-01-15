using UnityEngine;
using _Project.Scripts.Domain;

namespace _Project.Scripts.Presentation
{
    public sealed class VFXManager : MonoBehaviour
    {
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
        }

        private void Unsubscribe()
        {
            if (context == null) return;

            context.events.onDecisionOutcome -= OnDecisionOutcome;
        }

        private void OnDecisionOutcome(DecisionOutcomeData data)
        {
            if (data.wasCorrect) PlayEffect("Correct");
            else PlayEffect("Wrong");
        }

        public void PlayEffect(string effectId)
        {
            //TODO: Mögliche Animationen, Lichter
        }
    }
}