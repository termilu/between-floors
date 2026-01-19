using UnityEngine;

public class ElevatorDecisionButton : MonoBehaviour
{
    public SimpleElevatorDoor doorController;
    public GameManager.ElevatorChoice choice;

    [Header("Trigger Filtering")]
    public string ignoreRootName = "Elevator";

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (other.transform.root.name == ignoreRootName)
            return;

        used = true;

        if (GameManager.Instance == null)
        {
            Debug.LogError("[ElevatorDecisionButton] GameManager missing!");
            return;
        }

        GameManager.Instance.SetPendingElevatorChoice(choice);

        if (doorController != null)
            doorController.OpenDoors();
        else
            Debug.LogWarning("[ElevatorDecisionButton] doorController is NOT assigned!");
    }
}
