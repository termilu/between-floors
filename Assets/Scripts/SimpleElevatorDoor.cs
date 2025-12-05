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
    public float autoCloseDelay = 5f;
    
    Vector3 insideLeftClosed;
    Vector3 insideRightClosed;
    Vector3 outsideLeftClosed;
    Vector3 outsideRightClosed;

    Vector3 insideLeftOpen;
    Vector3 insideRightOpen;
    Vector3 outsideLeftOpen;
    Vector3 outsideRightOpen;

    Coroutine currentRoutine;

    void Awake()
    {
        insideLeftClosed   = insideLeft.localPosition;
        insideRightClosed  = insideRight.localPosition;
        outsideLeftClosed  = outsideLeft.localPosition;
        outsideRightClosed = outsideRight.localPosition;
        
        insideLeftOpen   = insideLeftClosed   + Vector3.left  * openDistance;
        insideRightOpen  = insideRightClosed  + Vector3.right * openDistance;
        outsideLeftOpen  = outsideLeftClosed  + Vector3.left  * openDistance;
        outsideRightOpen = outsideRightClosed + Vector3.right * openDistance;
    }

    public void OpenDoors()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(OpenRoutine());
    }

    public void CloseDoors()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(CloseRoutine());
    }

    IEnumerator OpenRoutine()
    {
        Debug.Log("[SimpleElevatorDoor] Opening doors...");

        while (true)
        {
            insideLeft.localPosition   = Vector3.Lerp(insideLeft.localPosition,   insideLeftOpen,   doorSpeed * Time.deltaTime);
            insideRight.localPosition  = Vector3.Lerp(insideRight.localPosition,  insideRightOpen,  doorSpeed * Time.deltaTime);
            outsideLeft.localPosition  = Vector3.Lerp(outsideLeft.localPosition,  outsideLeftOpen,  doorSpeed * Time.deltaTime);
            outsideRight.localPosition = Vector3.Lerp(outsideRight.localPosition, outsideRightOpen, doorSpeed * Time.deltaTime);

            if (Vector3.Distance(insideLeft.localPosition, insideLeftOpen) < 0.001f)
                break;

            yield return null;
        }
        
        insideLeft.localPosition   = insideLeftOpen;
        insideRight.localPosition  = insideRightOpen;
        outsideLeft.localPosition  = outsideLeftOpen;
        outsideRight.localPosition = outsideRightOpen;

        yield return new WaitForSeconds(autoCloseDelay);
        currentRoutine = StartCoroutine(CloseRoutine());
    }

    IEnumerator CloseRoutine()
    {
        Debug.Log("[SimpleElevatorDoor] Closing doors...");

        while (true)
        {
            insideLeft.localPosition   = Vector3.Lerp(insideLeft.localPosition,   insideLeftClosed,   doorSpeed * Time.deltaTime);
            insideRight.localPosition  = Vector3.Lerp(insideRight.localPosition,  insideRightClosed,  doorSpeed * Time.deltaTime);
            outsideLeft.localPosition  = Vector3.Lerp(outsideLeft.localPosition,  outsideLeftClosed,  doorSpeed * Time.deltaTime);
            outsideRight.localPosition = Vector3.Lerp(outsideRight.localPosition, outsideRightClosed, doorSpeed * Time.deltaTime);

            if (Vector3.Distance(insideLeft.localPosition, insideLeftClosed) < 0.001f)
                break;

            yield return null;
        }
        
        insideLeft.localPosition   = insideLeftClosed;
        insideRight.localPosition  = insideRightClosed;
        outsideLeft.localPosition  = outsideLeftClosed;
        outsideRight.localPosition = outsideRightClosed;
    }
}
