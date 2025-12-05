using UnityEngine;

public class SimpleAnomalyScreen : MonoBehaviour
{
    public Material normalMat;
    public Material offMat;
    public Material anomalyMat;
    public bool isAnomalyScreen = false;
    public GameObject screen;

    private Renderer rend;
    private Material[] mats;
    private bool isOn = true;
    private bool anomalyTriggered = false;

    void Start()
    {
        if (screen == null) screen = gameObject;

        rend = screen.GetComponent<Renderer>();
        mats = rend.materials;
        SetScreenMaterial(normalMat);
    }

    public void Interact()
    {
        if (!isOn && !isAnomalyScreen) return;

        if (isAnomalyScreen && !anomalyTriggered)
        {
            anomalyTriggered = true;
            SetScreenMaterial(anomalyMat);
        }
        else
        {
            isOn = false;
            SetScreenMaterial(offMat);
        }
    }

    private void SetScreenMaterial(Material mat)
    {
        if (mats == null || mats.Length == 0) return;
        mats[1] = mat;               // assumes index 1 is the screen slot
        rend.materials = mats;
    }
}
