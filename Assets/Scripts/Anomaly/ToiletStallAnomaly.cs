using System.Collections;
using UnityEngine;

public class ToiletStallAnomaly : MonoBehaviour
{
    [Header("References")]

    // Transform of the stall door that will rotate
    public Transform door;

    // AudioSource placed inside the stall
    public AudioSource insideAudio;


    [Header("Door Rotation (Local Space)")]

    // Local rotation of the door when it is open
    public Vector3 openEuler = new Vector3(0f, 0f, -75f);

    // Local rotation of the door when it is closed
    public Vector3 closedEuler = new Vector3(0f, 0f, 0f);


    [Header("Timing")]

    // How fast the door slams shut
    public float slamDuration = 0.12f;

    // Audio delay after door slam
    public float audioDelay = 0.15f;


    [Header("Trigger Settings")]

    // Only objects with this tag can trigger the anomaly
    public string triggerTag = "Player";

    // If true, the anomaly can only be triggered once
    public bool triggerOnce = true;


    private bool hasTriggered = false;

    private Quaternion openRotation;
    private Quaternion closedRotation;


    private void Awake()
    {
        // Cache rotations for performance
        openRotation = Quaternion.Euler(openEuler);
        closedRotation = Quaternion.Euler(closedEuler);
    }

    private void Start()
    {
        // Ensure the door starts in the open position
        if (door != null)
        {
            door.localRotation = openRotation;
        }

        // Ensure audio does not play automatically
        if (insideAudio != null)
        {
            insideAudio.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Prevent re-triggering if only one activation is allowed
        if (triggerOnce && hasTriggered)
            return;

        // Check tag if required
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag))
            return;

        hasTriggered = true;
        StartCoroutine(SlamDoorAndPlayAudio());
    }

    private IEnumerator SlamDoorAndPlayAudio()
    {
        // Slam the door shut
        if (door != null)
        {
            Quaternion startRotation = door.localRotation;
            Quaternion endRotation = closedRotation;

            float elapsed = 0f;

            while (elapsed < slamDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / slamDuration);

                // Slam effect using an ease-out curve
                float easedT = 1f - Mathf.Pow(1f - t, 6f);

                door.localRotation = Quaternion.Slerp(startRotation, endRotation, easedT);
                yield return null;
            }

            door.localRotation = endRotation;
        }

        // Optional delay before audio playback
        if (audioDelay > 0f)
        {
            yield return new WaitForSeconds(audioDelay);
        }

        // Play the sound inside the stall
        if (insideAudio != null)
        {
            insideAudio.Play();
        }
    }
}
