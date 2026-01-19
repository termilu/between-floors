using System.Collections;
using UnityEngine;

public class ToiletStallAnomaly : MonoBehaviour, IArmedAnomaly
{
    [Header("References")]
    public Transform door;
    public AudioSource slamAudio;
    public AudioSource insideAudio;

    [Header("Door Rotation (Local)")]
    public Vector3 openEuler = new Vector3(0f, 0f, -75f);
    public Vector3 closedEuler = new Vector3(0f, 0f, 0f);

    [Header("Timing")]
    public float closeDuration = 0.25f;
    public float insideAudioDelay = 0.15f;

    [Header("Trigger Settings")]
    public string triggerTag = "Player";
    public bool triggerOnce = true;

    private bool hasTriggered = false;
    private Quaternion openRotation;
    private Quaternion closedRotation;

    private Collider triggerCol;
    private Coroutine runningRoutine;

    private void Awake()
    {
        triggerCol = GetComponent<Collider>();
        if (triggerCol == null)
        {
            Debug.LogError($"{name}: No Collider found on the trigger object.");
            enabled = false;
            return;
        }

        if (door == null)
        {
            Debug.LogError($"{name}: Door reference missing.");
            enabled = false;
            return;
        }

        openRotation = Quaternion.Euler(openEuler);
        closedRotation = Quaternion.Euler(closedEuler);

        if (slamAudio != null) slamAudio.playOnAwake = false;
        if (insideAudio != null) insideAudio.playOnAwake = false;

        // Default to clean state and disarmed
        ResetToDefault();
        SetArmed(false);
    }

    public void SetArmed(bool armed)
    {
        // Reset per round
        hasTriggered = false;

        // Stop anything in progress
        if (runningRoutine != null)
        {
            StopCoroutine(runningRoutine);
            runningRoutine = null;
        }

        // Stop audio if disarming (or rearming)
        if (slamAudio != null && slamAudio.isPlaying) slamAudio.Stop();
        if (insideAudio != null && insideAudio.isPlaying) insideAudio.Stop();

        // Always return the door to open at round start / disarm
        ResetToDefault();

        triggerCol.enabled = armed;
    }

    private void ResetToDefault()
    {
        if (door != null)
            door.localRotation = openRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCol.enabled) return;
        if (triggerOnce && hasTriggered) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;

        hasTriggered = true;

        if (runningRoutine != null)
            StopCoroutine(runningRoutine);

        runningRoutine = StartCoroutine(CloseDoorThenPlayAudio());
    }

    private IEnumerator CloseDoorThenPlayAudio()
    {
        Quaternion startRotation = door.localRotation;
        Quaternion endRotation = closedRotation;

        float elapsed = 0f;

        while (elapsed < closeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / closeDuration);
            float easedT = SmoothStep01(t);

            door.localRotation = Quaternion.Slerp(startRotation, endRotation, easedT);
            yield return null;
        }

        door.localRotation = endRotation;

        if (slamAudio != null)
            slamAudio.Play();

        if (insideAudio != null)
        {
            if (insideAudioDelay > 0f)
                yield return new WaitForSeconds(insideAudioDelay);

            insideAudio.Play();
        }

        runningRoutine = null;
    }

    private float SmoothStep01(float t) => t * t * (3f - 2f * t);
}
