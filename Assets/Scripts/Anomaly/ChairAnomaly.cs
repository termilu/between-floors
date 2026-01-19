using System.Collections;
using UnityEngine;

public class ChairAnomaly : MonoBehaviour, IArmedAnomaly
{
    [Header("References")]
    public Transform chair;

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

        if (chair == null)
        {
            Debug.LogError($"{name}: Chair reference missing.");
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
            startPos = chair.localPosition;
            startRot = chair.localRotation;
        }
        else
        {
            startPos = chair.position;
            startRot = chair.rotation;
        }
    }

    public void SetArmed(bool armed)
    {
        // Stop any pending revert
        if (revertRoutine != null)
        {
            StopCoroutine(revertRoutine);
            revertRoutine = null;
        }

        // Reset state
        anomalyTriggered = false;

        // Always revert when disarming (or rearming) to keep rounds clean
        RevertNow();

        // Arm/disarm trigger collider
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
            chair.localPosition = targetPosition;
            chair.localRotation = targetRot;
        }
        else
        {
            chair.position = targetPosition;
            chair.rotation = targetRot;
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
        if (chair == null) return;

        if (useLocalSpace)
        {
            chair.localPosition = startPos;
            chair.localRotation = startRot;
        }
        else
        {
            chair.position = startPos;
            chair.rotation = startRot;
        }
    }
}
