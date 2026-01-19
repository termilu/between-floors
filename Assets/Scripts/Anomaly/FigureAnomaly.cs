using System.Collections;
using UnityEngine;

public class FigureAnomaly : MonoBehaviour, IArmedAnomaly
{
    [Header("References")]
    public GameObject figure;

    [Header("Timing")]
    public float visibleDuration = 2f;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Trigger Settings")]
    public string triggerTag = "Player";
    public bool triggerOnce = true;

    private bool anomalyTriggered = false;
    private Collider triggerCol;

    private void Awake()
    {
        triggerCol = GetComponent<Collider>();
        if (triggerCol == null)
        {
            Debug.LogError($"{name}: No Collider found on the trigger object.");
            enabled = false;
            return;
        }

        if (figure == null)
        {
            Debug.LogError($"{name}: Figure reference missing.");
            enabled = false;
            return;
        }

        // Visual should start hidden always; round logic only arms the trigger
        figure.SetActive(false);

        // Safety: audio must be 3D
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1f; // 3D
        }

        // Default: disarmed (RoundBootstrap will arm one if needed)
        SetArmed(false);
    }

    public void SetArmed(bool armed)
    {
        // Reset per round
        anomalyTriggered = false;

        // If you disarm, also ensure it’s hidden
        if (!armed && figure.activeSelf)
            figure.SetActive(false);

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        triggerCol.enabled = armed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCol.enabled) return; // extra safety
        if (triggerOnce && anomalyTriggered) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;

        anomalyTriggered = true;
        StartCoroutine(ShowTemporarily());
    }

    private IEnumerator ShowTemporarily()
    {
        figure.SetActive(true);

        if (audioSource != null)
            audioSource.Play();

        yield return new WaitForSeconds(visibleDuration);
        figure.SetActive(false);
    }
}
