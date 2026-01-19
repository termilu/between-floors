using UnityEngine;

public class SimpleAnomalyScreen : MonoBehaviour, IArmedAnomaly
{
    [Header("Materials")]
    public Material normalMat;
    public Material offMat;
    public Material anomalyMat; // BieneMaja

    [Header("Renderer")]
    public GameObject screen; // object that has the Renderer

    [Header("Assumes screen material is at this index in Renderer.materials")]
    public int screenMaterialIndex = 1;

    private Renderer rend;
    private Material[] mats;

    public bool isArmed = false;
    private bool interactedThisRound = false;

    private void Awake()
    {
        if (screen == null) screen = gameObject;

        rend = screen.GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError($"{name}: No Renderer found on screen object.");
            enabled = false;
            return;
        }

        mats = rend.materials;

        // Default: clean normal state
        SetScreenMaterial(normalMat);
        interactedThisRound = false;
        isArmed = false;
    }

    // Called by RoundBootstrapTriggers
    public void SetArmed(bool armed)
    {
        isArmed = armed;
        interactedThisRound = false;

        // Always start each round in the normal state
        SetScreenMaterial(normalMat);
    }

    // Called by keyboard trigger
    public void Interact()
    {
        // simplest: only allow one interaction per round
        if (interactedThisRound) return;
        interactedThisRound = true;

        if (isArmed)
        {
            // anomaly round -> show BieneMaja
            SetScreenMaterial(anomalyMat);
        }
        else
        {
            // no anomaly round -> turn off
            SetScreenMaterial(offMat);
        }
    }

    private void SetScreenMaterial(Material mat)
    {
        if (mats == null || mats.Length == 0) return;
        if (screenMaterialIndex < 0 || screenMaterialIndex >= mats.Length)
        {
            Debug.LogError($"{name}: screenMaterialIndex {screenMaterialIndex} is out of range (materials length = {mats.Length}).");
            return;
        }

        mats[screenMaterialIndex] = mat;
        rend.materials = mats;
    }
}
