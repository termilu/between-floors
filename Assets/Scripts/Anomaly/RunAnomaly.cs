using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAnomaly : MonoBehaviour
{
    public Material normalMat;
    public Material anomalyMat;
    public bool isAnomalyBoard = false;
    public GameObject board;

    private Renderer rend;
    private Material[] mats;
    private bool anomalyTriggered = false;
    void Start()
    {
        if (board == null) board = gameObject;

        rend = board.GetComponent<Renderer>();
        mats = rend.materials;
        SetBoardMaterial(normalMat);
    }

    public void TriggerAnomaly()
    {
        if (isAnomalyBoard && !anomalyTriggered)
        {
            anomalyTriggered = true;
            SetBoardMaterial(anomalyMat);
        }
    }

    private void SetBoardMaterial(Material mat)
    {
        if (mats == null || mats.Length == 0) return;
        mats[1] = mat;               
        rend.materials = mats;
    }
}
