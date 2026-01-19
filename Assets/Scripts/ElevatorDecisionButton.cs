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

        // Ignore elevator parts
        if (other.transform.root.name == ignoreRootName)
            return;

        Debug.Log($"[ElevatorDecisionButton] Triggered by {other.name} ({choice})");

        used = true;

        if (doorController != null)
            doorController.OpenDoors();
        else
            Debug.LogWarning("[ElevatorDecisionButton] doorController not assigned!");

        if (GameManager.Instance == null)
        {
            Debug.LogError("[ElevatorDecisionButton] GameManager missing!");
            return;
        }

        GameManager.Instance.SubmitElevatorChoice(choice);
    }
}
