using UnityEngine;

public class PicAnomaly : MonoBehaviour, IArmedAnomaly
{
    [Header("Materials")]
    public Material normalMat;
    public Material anomalyMat;

    [Header("Renderer")]
    public GameObject pic;                 // object with Renderer
    public int materialIndex = 1;          // which slot is the picture

    [Header("Trigger Settings")]
    public string triggerTag = "Player";
    public bool triggerOnce = true;

    private Renderer rend;
    private Material[] mats;

    private bool anomalyTriggered = false;
    private Collider triggerCol;

    private void Awake()
    {
        triggerCol = GetComponent<Collider>();
        if (triggerCol == null)
        {
            Debug.LogError($"{name}: No Collider found on the trigger object.");
            enabled = false;
            return;
        }

        if (pic == null) pic = gameObject;

        rend = pic.GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError($"{name}: No Renderer found on pic object.");
            enabled = false;
            return;
        }

        mats = rend.materials;

        // Start clean
        SetPicMaterial(normalMat);

        // Default: disarmed
        SetArmed(false);
    }

    public void SetArmed(bool armed)
    {
        anomalyTriggered = false;

        // Always reset to normal each round/disarm
        SetPicMaterial(normalMat);

        triggerCol.enabled = armed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCol.enabled) return;
        if (triggerOnce && anomalyTriggered) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;

        anomalyTriggered = true;
        SetPicMaterial(anomalyMat);
    }

    private void SetPicMaterial(Material mat)
    {
        if (mats == null || mats.Length == 0) return;

        if (materialIndex < 0 || materialIndex >= mats.Length)
        {
            Debug.LogError($"{name}: materialIndex {materialIndex} out of range (materials length = {mats.Length}).");
            return;
        }

        mats[materialIndex] = mat;
        rend.materials = mats;
    }
}
