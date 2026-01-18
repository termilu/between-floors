using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairAnomaly : MonoBehaviour
{
    public GameObject chair;
    [SerializeField] private bool anomalyTriggered = false;

    void Start()
    {
       Levitate();
    }

    public void Levitate()
    { 
        if(!anomalyTriggered)
        {
            chair.transform.position =  new Vector3(1.251839f,2.123f,-63.62834f);
            chair.transform.eulerAngles = new Vector3(90,180,0); 
        }
    }
}
