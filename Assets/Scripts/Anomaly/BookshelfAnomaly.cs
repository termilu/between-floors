using System.Collections;
using UnityEngine;

public class BookshelfAnomaly : MonoBehaviour, IArmedAnomaly
{
    [Header("References")]
    public Transform shelf;

    [Header("Target (Local or World)")]
    public bool useLocalSpace = false;
    public Vector3 targetPosition;
    public Vector3 targetEuler;

    [Header("Optional: Revert after seconds (<= 0 means never)")]
    public float revertAfter = 0f;

    [Header("Trigger Settings")]
    public string triggerTag = "Player";
    public bool triggerOnce = true;

    private bool anomalyTriggered = false;

    private Vector3 startPos;
    private Quaternion startRot;

    private Collider triggerCol;
    private Coroutine revertRoutine;

    private void Awake()
    {
        triggerCol = GetComponent<Collider>();
        if (triggerCol == null)
        {
            Debug.LogError($"{name}: No Collider found on the trigger object.");
            enabled = false;
            return;
        }

        if (shelf == null)
        {
            Debug.LogError($"{name}: Shelf reference missing.");
            enabled = false;
            return;
        }

        CacheStartTransform();

        // Default: disarmed (RoundBootstrap arms one if needed)
        SetArmed(false);
    }

    private void CacheStartTransform()
    {
        if (useLocalSpace)
        {
            startPos = shelf.localPosition;
            startRot = shelf.localRotation;
        }
        else
        {
            startPos = shelf.position;
            startRot = shelf.rotation;
        }
    }

    public void SetArmed(bool armed)
    {
        if (revertRoutine != null)
        {
            StopCoroutine(revertRoutine);
            revertRoutine = null;
        }

        anomalyTriggered = false;

        // Keep it clean between rounds
        RevertNow();

        triggerCol.enabled = armed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCol.enabled) return;
        if (triggerOnce && anomalyTriggered) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;

        anomalyTriggered = true;
        ApplyLevitate();

        if (revertAfter > 0f)
            revertRoutine = StartCoroutine(RevertAfterSeconds());
    }

    private void ApplyLevitate()
    {
        Quaternion targetRot = Quaternion.Euler(targetEuler);

        if (useLocalSpace)
        {
            shelf.localPosition = targetPosition;
            shelf.localRotation = targetRot;
        }
        else
        {
            shelf.position = targetPosition;
            shelf.rotation = targetRot;
        }
    }

    private IEnumerator RevertAfterSeconds()
    {
        yield return new WaitForSeconds(revertAfter);
        RevertNow();
        revertRoutine = null;
    }

    private void RevertNow()
    {
        if (shelf == null) return;

        if (useLocalSpace)
        {
            shelf.localPosition = startPos;
            shelf.localRotation = startRot;
        }
        else
        {
            shelf.position = startPos;
            shelf.rotation = startRot;
        }
    }
}
