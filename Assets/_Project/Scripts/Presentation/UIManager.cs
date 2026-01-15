using UnityEngine;
using _Project.Scripts.Domain.States;
using _Project.Scripts.Data;

namespace _Project.Scripts.Presentation
{
    public sealed class UIManager : MonoBehaviour
    {
        private GameContext context;

        public void Initialize(GameContext ctx)
        {
            context = ctx;
        }
        
        public void OnStartGame()
        {
            if (context == null) return;

            if (context.stateMachine.currentState is MainMenuState menu)
            {
                menu.StartGame(context.flow);
            }
        }

        public void OnPlayerReady()
        {
            if (context == null) return;

            if (context.stateMachine.currentState is MemorizeState mem)
            {
                mem.PlayerReady();
            }
        }
        
        public void OnSetTeleport()
        {
            if (context == null) return;
            context.flow.SetMovementType(MovementType.Teleport);
        }

        public void OnSetSmooth()
        {
            if (context == null) return;
            context.flow.SetMovementType(MovementType.Smooth);
        }
    }
}
