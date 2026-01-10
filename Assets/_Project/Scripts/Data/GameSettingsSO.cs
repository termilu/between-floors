using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(menuName = "VRAR/Data/Game Settings", fileName = "GameSettings")]
    public class GameSettingsSO : ScriptableObject
    {
        [Header("Highest Floor")]
        public int maxFloors = 9; //Muss noch besprochen werden
        
        [Header("Movement")]
        public Enums.MovementType defaultMovementType = Enums.MovementType.Teleport;
        
        [Header("Feedback")]
        public float resultDurationSeconds = 2.0f; //hilfreich für Feedback-Animationen etc.
    }
}