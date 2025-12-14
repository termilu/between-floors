using UnityEngine;

public class ElevatorDoorTriggerButton : MonoBehaviour
{
    public SimpleElevatorDoor doorController;

    private void OnTriggerEnter(Collider other)
    {
        // I know this is horrible, but it works LOL
        if (other.transform.root.name == "Elevator")
        {
            return;
        }
        
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