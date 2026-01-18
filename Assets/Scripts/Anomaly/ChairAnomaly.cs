using System.Collections;
using UnityEngine;

public class ChairAnomaly : MonoBehaviour
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

    private void Awake()
    {
        if (chair == null)
        {
            Debug.LogError($"{name}: Chair reference missing.");
            enabled = false;
            return;
        }

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
