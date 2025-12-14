using System.Collections;
using UnityEngine;

public class ToiletStallAnomaly : MonoBehaviour
{
    [Header("References")]

    // Transform of the stall door pivot that will rotate
    public Transform door;

    // Short slam SFX (plays when the door reaches the closed state)
    public AudioSource slamAudio;

    // Creepy inside audio (plays after a delay, optional)
    public AudioSource insideAudio;


    [Header("Door Rotation (Local Space)")]

    public Vector3 openEuler = new Vector3(0f, 0f, -75f);
    public Vector3 closedEuler = new Vector3(0f, 0f, 0f);


    [Header("Timing")]

    // Total time for the door closing animation (seconds)
    public float closeDuration = 0.25f;

    // Optional delay after the slam before inside audio starts
    public float insideAudioDelay = 0.15f;


    [Header("Trigger Settings")]

    public string triggerTag = "Player";
    public bool triggerOnce = true;


    private bool hasTriggered = false;
    private Quaternion openRotation;
    private Quaternion closedRotation;

    private void Awake()
    {
        openRotation = Quaternion.Euler(openEuler);
        closedRotation = Quaternion.Euler(closedEuler);
    }

    private void Start()
    {
        if (door != null)
            door.localRotation = openRotation;

        if (slamAudio != null) slamAudio.playOnAwake = false;
        if (insideAudio != null) insideAudio.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && hasTriggered) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;

        hasTriggered = true;
        StartCoroutine(CloseDoorThenPlayAudio());
    }

    private IEnumerator CloseDoorThenPlayAudio()
    {
        if (door == null)
        {
            Debug.LogError("[ToiletStallAnomaly] Door reference is missing.");
            yield break;
        }

        Quaternion startRotation = door.localRotation;
        Quaternion endRotation = closedRotation;

        float elapsed = 0f;

        while (elapsed < closeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / closeDuration);

            // Ease-in + ease-out for a more natural close
            float easedT = SmoothStep01(t);

            door.localRotation = Quaternion.Slerp(startRotation, endRotation, easedT);
            yield return null;
        }

        // Ensure we end exactly at the closed rotation
        door.localRotation = endRotation;

        // Play the slam sound immediately after reaching the closed state (same frame)
        if (slamAudio != null)
            slamAudio.Play();

        // Optionally play inside audio after a delay
        if (insideAudio != null)
        {
            if (insideAudioDelay > 0f)
                yield return new WaitForSeconds(insideAudioDelay);

            insideAudio.Play();
        }
    }

    private float SmoothStep01(float t)
    {
        // Standard smoothstep (ease-in-out) in range [0..1]
        return t * t * (3f - 2f * t);
    }
}
