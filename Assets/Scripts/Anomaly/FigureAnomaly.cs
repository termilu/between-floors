using System.Collections;
using UnityEngine;

public class FigureAnomaly : MonoBehaviour
{
    [Header("References")]
    public GameObject figure;

    [Header("Timing")]
    public float visibleDuration = 2f;

    [Header("Trigger Settings")]
    public string triggerTag = "Player";
    public bool triggerOnce = true;

    private bool anomalyTriggered = false;

    private void Awake()
    {
        if (figure == null)
        {
            Debug.LogError($"{name}: Figure reference missing.");
            enabled = false;
            return;
        }

        figure.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && anomalyTriggered) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;

        anomalyTriggered = true;
        StartCoroutine(ShowTemporarily());
    }
    private IEnumerator ShowTemporarily()
    {
        figure.SetActive(true);
        yield return new WaitForSeconds(visibleDuration);
        figure.SetActive(false);
    }
}