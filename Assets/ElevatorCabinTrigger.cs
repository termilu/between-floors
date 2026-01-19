using UnityEngine;

public class ElevatorCabinTrigger : MonoBehaviour
{
    public string triggerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag))
            return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("[ElevatorCabinTrigger] GameManager missing!");
            return;
        }

        GameManager.Instance.CommitPendingChoiceAndLoadNextRound();
    }
}
