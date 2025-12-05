using UnityEngine;

public class ElevatorDoorTriggerButton : MonoBehaviour
{
    public SimpleElevatorDoor doorController;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[ElevatorDoorTriggerButton] TRIGGER from: " + other.name);

        if (doorController != null)
        {
            doorController.OpenDoors();
        }
        else
        {
            Debug.LogWarning("[ElevatorDoorTriggerButton] doorController is NOT assigned!");
        }
    }
}