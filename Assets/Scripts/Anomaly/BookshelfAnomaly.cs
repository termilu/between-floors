using System.Collections;
using UnityEngine;

public class BookshelfAnomaly : MonoBehaviour
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

    private void Awake()
    {
        if (shelf == null)
        {
            Debug.LogError($"{name}: Shelf reference missing.");
            enabled = false;
            return;
        }

        // Cache initial transform so we can revert reliably
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

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && anomalyTriggered) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;

        anomalyTriggered = true;
        ApplyLevitate();

        if (revertAfter > 0f)
            StartCoroutine(RevertAfterSeconds());
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
