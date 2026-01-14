using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Data;
using _Project.Scripts.Domain;
using _Project.Scripts.Domain.States;
using _Project.Scripts.Presentation.Movement;

namespace _Project.Scripts.Presentation
{
    public sealed class SceneBootstrapper : MonoBehaviour
    {
        [Header("Data Layer (Assets)")]
        [SerializeField] private GameSettingsSO gameSettings;
        [SerializeField] private List<FloorConfigSO> floors = new();
        
        [Header("Scene References")]
        [SerializeField] private VRPlayerController playerController;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private HUDController hudController;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private VFXManager vfxManager;
        [SerializeField] private AnomalyViewSpawner anomalyViewSpawner;
        [SerializeField] private ElevatorController elevatorController;
        
        public static SceneBootstrapper instance { get; private set; }
        public GameContext context { get; private set; }
        
        private MainMenuState mainMenuState;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            ConstructDomain();
            BindManagers();
        }

        private void Update()
        {
            if (context == null) return;

            context.stateMachine.Update();

            if (context.stateMachine.currentState is ResultState resultState)
            {
                resultState.Tick(Time.deltaTime);
            }
        }

        private void ConstructDomain()
        {
            if (gameSettings == null)
            {
                Debug.LogError("[SceneBootstrapper] GameSettingsSO fehlt im Inspector.");
            }
            
            var eventsHub = new GameEventsHub();
            var stateMachine = new GameStateMachine(eventsHub);
            var anomalyService = new AnomalyService(); 
            
            var flow = new GameFlowService(
                settings: gameSettings,
                floorConfigs: floors,
                anomalies: anomalyService,
                events: eventsHub);
            
            context = new GameContext(eventsHub, stateMachine, flow, anomalyService);

            mainMenuState = new MainMenuState(stateMachine);
            stateMachine.changeState(mainMenuState);
        }

        private void BindManagers()
        {
            if (context == null) return;
            
            if (playerController != null)
            {
                playerController.Initialize(context);
            }
            
            if (uiManager != null)
            {
                uiManager.Initialize(context);
            }

            if (hudController != null)
            {
                hudController.Initialize(context);
            }
            
            if (audioManager != null)
            {
                audioManager.Initialize(context);
            }

            if (vfxManager != null)
            {
                vfxManager.Initialize(context);
            }
            
            if (anomalyViewSpawner != null)
            {
                anomalyViewSpawner.Initialize(context, floors);
            }
            
            if (elevatorController != null)
            {
                elevatorController.Initialize(context);
            }
        }

        public void StartGameFromUI()
        {
            if (context == null) return;

            if (context.stateMachine.currentState is MainMenuState menu)
            {
                menu.StartGame(context.flow);
            }
        }

        public void PlayerReadyFromUI()
        {
            if (context == null) return;

            if (context.stateMachine.currentState is MemorizeState mem)
            {
                mem.PlayerReady();
            }
        }
    }
}