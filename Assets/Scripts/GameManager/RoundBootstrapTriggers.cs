using UnityEngine;

public class RoundBootstrapTriggers : MonoBehaviour
{
    [Header("Drag ALL anomaly trigger components here (the scripts on the trigger objects)")]
    public MonoBehaviour[] anomalyTriggers; // must implement IArmedAnomaly

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager missing. If testing OfficeScene directly, place a GameManager in the scene.");
            return;
        }

        // Allow pressing Play from OfficeScene
        if (!GameManager.Instance.HasRound)
            GameManager.Instance.GenerateNextRound();

        // Disarm everything
        for (int i = 0; i < anomalyTriggers.Length; i++)
        {
            var t = anomalyTriggers[i] as IArmedAnomaly;
            if (t != null) t.SetArmed(false);
            else if (anomalyTriggers[i] != null)
                Debug.LogWarning($"{anomalyTriggers[i].name} does not implement IArmedAnomaly.");
        }

        // If no-anomaly round, stop here
        if (!GameManager.Instance.RoundHasAnomaly)
            return;

        // Pick exactly one anomaly and arm it
        if (anomalyTriggers == null || anomalyTriggers.Length == 0)
            return;

        int idx = Random.Range(0, anomalyTriggers.Length);
        var chosen = anomalyTriggers[idx] as IArmedAnomaly;
        if (chosen != null)
            chosen.SetArmed(true);
    }
}