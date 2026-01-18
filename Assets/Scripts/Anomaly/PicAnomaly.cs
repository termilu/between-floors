using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicAnomaly : MonoBehaviour
{   
    public Material normalMat;
    public Material anomalyMat;
    public bool isAnomalyPic = false;
    public GameObject pic;

    private Renderer rend;
    private Material[] mats;
    private bool anomalyTriggered = false;
    void Start()
    {
        if (pic == null) pic = gameObject;

        rend = pic.GetComponent<Renderer>();
        mats = rend.materials;
        SetPicMaterial(normalMat);
    }

    public void TriggerAnomaly()
    {
        if (isAnomalyPic && !anomalyTriggered)
        {
            anomalyTriggered = true;
            SetPicMaterial(anomalyMat);
        }
    }

    private void SetPicMaterial(Material mat)
    {
        if (mats == null || mats.Length == 0) return;
        mats[1] = mat;               
        rend.materials = mats;
    }
}
