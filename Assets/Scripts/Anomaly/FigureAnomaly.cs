using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureAnomaly : MonoBehaviour
{
    public GameObject figure;
    private bool anomalyTriggered = false;
    void Start()
    {
        figure.SetActive(false);
    }

    public void Appear()
    {
        if(!anomalyTriggered)
        {
            figure.SetActive(true);
        }
    }
}
