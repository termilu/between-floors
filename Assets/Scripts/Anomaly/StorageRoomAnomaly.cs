using System.Collections;
using UnityEngine;

public class StorageRoomAnomaly : MonoBehaviour, IArmedAnomaly
{
    [Header("References")]
    public GameObject ghost;
    public AudioSource audioSource;

    [Header("Timing")]
    public float visibleDuration = 2f;

    [Header("Trigger Settings")]
    public string triggerTag = "Player";
    public bool triggerOnce = true;

    private bool anomalyTriggered = false;
    private Collider triggerCol;
    private Coroutine routine;

    private void Awake()
    {
        triggerCol = GetComponent<Collider>();
        if (triggerCol == null)
        {
            Debug.LogError($"{name}: No Collider found on the trigger object.");
            enabled = false;
            return;
        }

        if (ghost == null)
        {
            Debug.LogError($"{name}: Ghost reference missing.");
            enabled = false;
            return;
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1f; // 3D sound at source position
        }

        ghost.SetActive(false);
        SetArmed(false);
    }

    public void SetArmed(bool armed)
    {
        anomalyTriggered = false;

        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        ghost.SetActive(false);

        triggerCol.enabled = armed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCol.enabled) return;
        if (triggerOnce && anomalyTriggered) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;

        anomalyTriggered = true;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShowTemporarily());
    }

    private IEnumerator ShowTemporarily()
    {
        ghost.SetActive(true);

        if (audioSource != null)
            audioSource.Play();

        yield return new WaitForSeconds(visibleDuration);

        ghost.SetActive(false);
        routine = null;
    }
}
