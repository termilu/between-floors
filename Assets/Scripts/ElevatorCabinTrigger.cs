using UnityEngine;

public class ElevatorCabinTrigger : MonoBehaviour
{
    public string triggerTag = "Player";
    public SimpleElevatorDoor doorController;
    public float closeDelay = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag))
            return;

        // Cancel any pending close (e.g. if player re-enters quickly)
        if (doorController != null)
            doorController.CancelScheduledClose();

        if (GameManager.Instance == null)
        {
            Debug.LogError("[ElevatorCabinTrigger] GameManager missing!");
            return;
        }

        // Only commit the round if the player pressed Up/Down
        if (!GameManager.Instance.HasPendingElevatorChoice)
            return;

        GameManager.Instance.CommitPendingChoiceAndLoadNextRound();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag))
            return;

        // Close 2s after leaving elevator
        if (doorController != null)
            doorController.ScheduleClose(closeDelay);
    }
}
