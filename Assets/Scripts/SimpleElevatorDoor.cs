using System.Collections;
using UnityEngine;

public class SimpleElevatorDoor : MonoBehaviour
{
    [Header("Doors (one floor)")]
    public Transform insideLeft;
    public Transform insideRight;
    public Transform outsideLeft;
    public Transform outsideRight;

    [Header("Settings")]
    public float openDistance = 0.01f;
    public float doorSpeed = 3f;

    [Header("Automatic close after opening (optional)")]
    public bool autoCloseEnabled = true;
    public float autoCloseDelay = 5f;

    private Vector3 insideLeftClosed;
    private Vector3 insideRightClosed;
    private Vector3 outsideLeftClosed;
    private Vector3 outsideRightClosed;

    private Vector3 insideLeftOpen;
    private Vector3 insideRightOpen;
    private Vector3 outsideLeftOpen;
    private Vector3 outsideRightOpen;

    private Coroutine moveRoutine;
    private Coroutine scheduledCloseRoutine;

    void Awake()
    {
        insideLeftClosed = insideLeft.localPosition;
        insideRightClosed = insideRight.localPosition;
        outsideLeftClosed = outsideLeft.localPosition;
        outsideRightClosed = outsideRight.localPosition;

        insideLeftOpen = insideLeftClosed + Vector3.left * openDistance;
        insideRightOpen = insideRightClosed + Vector3.right * openDistance;
        outsideLeftOpen = outsideLeftClosed + Vector3.left * openDistance;
        outsideRightOpen = outsideRightClosed + Vector3.right * openDistance;
    }

    public void OpenDoors()
    {
        CancelScheduledClose();

        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(OpenRoutine());
    }

    public void CloseDoors()
    {
        CancelScheduledClose();

        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(CloseRoutine());
    }

    // Cancels any close that was scheduled via ScheduleClose(...)
    public void CancelScheduledClose()
    {
        if (scheduledCloseRoutine != null)
        {
            StopCoroutine(scheduledCloseRoutine);
            scheduledCloseRoutine = null;
        }
    }

    // Schedules a close after delay seconds (used by your cabin trigger).
    // This works even if autoCloseEnabled is false.
    public void ScheduleClose(float delay)
    {
        CancelScheduledClose();

        if (delay <= 0f)
        {
            CloseDoors();
            return;
        }

        scheduledCloseRoutine = StartCoroutine(CloseAfter(delay));
    }

    private IEnumerator CloseAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        scheduledCloseRoutine = null;
        CloseDoors();
    }

    private IEnumerator OpenRoutine()
    {
        Debug.Log("[SimpleElevatorDoor] Opening doors...");

        while (true)
        {
            insideLeft.localPosition = Vector3.Lerp(insideLeft.localPosition, insideLeftOpen, doorSpeed * Time.deltaTime);
            insideRight.localPosition = Vector3.Lerp(insideRight.localPosition, insideRightOpen, doorSpeed * Time.deltaTime);
            outsideLeft.localPosition = Vector3.Lerp(outsideLeft.localPosition, outsideLeftOpen, doorSpeed * Time.deltaTime);
            outsideRight.localPosition = Vector3.Lerp(outsideRight.localPosition, outsideRightOpen, doorSpeed * Time.deltaTime);

            if (Vector3.Distance(insideLeft.localPosition, insideLeftOpen) < 0.001f)
                break;

            yield return null;
        }

        insideLeft.localPosition = insideLeftOpen;
        insideRight.localPosition = insideRightOpen;
        outsideLeft.localPosition = outsideLeftOpen;
        outsideRight.localPosition = outsideRightOpen;

        // Only the "automatic" close uses this toggle.
        // Your cabin trigger can still call ScheduleClose(...) regardless.
        if (autoCloseEnabled)
            ScheduleClose(autoCloseDelay);

        moveRoutine = null;
    }

    private IEnumerator CloseRoutine()
    {
        Debug.Log("[SimpleElevatorDoor] Closing doors...");

        while (true)
        {
            insideLeft.localPosition = Vector3.Lerp(insideLeft.localPosition, insideLeftClosed, doorSpeed * Time.deltaTime);
            insideRight.localPosition = Vector3.Lerp(insideRight.localPosition, insideRightClosed, doorSpeed * Time.deltaTime);
            outsideLeft.localPosition = Vector3.Lerp(outsideLeft.localPosition, outsideLeftClosed, doorSpeed * Time.deltaTime);
            outsideRight.localPosition = Vector3.Lerp(outsideRight.localPosition, outsideRightClosed, doorSpeed * Time.deltaTime);

            if (Vector3.Distance(insideLeft.localPosition, insideLeftClosed) < 0.001f)
                break;

            yield return null;
        }

        insideLeft.localPosition = insideLeftClosed;
        insideRight.localPosition = insideRightClosed;
        outsideLeft.localPosition = outsideLeftClosed;
        outsideRight.localPosition = outsideRightClosed;

        moveRoutine = null;
    }
}
