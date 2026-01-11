using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(menuName = "VRAR/Data/Game Settings", fileName = "GameSettings")]
    public class GameSettingsSO : ScriptableObject
    {
        [Header("Highest Floor")]
        public int maxFloors = 9; //Muss noch besprochen werden

        [Header("Highest Floor")] 
        public int maxFails = 3; //Macht Spiel schwere/spannender
        
        [Header("Movement")]
        public MovementType defaultMovementType = MovementType.Teleport;
        
        [Header("Feedback")]
        public float resultDurationSeconds = 2.0f; //Hilfreich für Feedback-Animationen etc.
    }
}