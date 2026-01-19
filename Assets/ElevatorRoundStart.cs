using UnityEngine;

public class ElevatorRoundStart : MonoBehaviour
{
    public Transform elevatorSpawn;
    public SimpleElevatorDoor doorController;

    [Tooltip("Assign your XR rig / XR Origin here.")]
    public Transform playerRig;

    private void Start()
    {
        if (GameManager.Instance != null && !GameManager.Instance.HasRound)
            GameManager.Instance.GenerateNextRound();

        if (playerRig != null && elevatorSpawn != null)
        {
            // Simple: move rig root to spawn
            playerRig.position = elevatorSpawn.position;
            playerRig.rotation = elevatorSpawn.rotation;
        }

        if (doorController != null)
            doorController.OpenDoors();
    }
}
