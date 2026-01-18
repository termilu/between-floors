using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfAnomaly : MonoBehaviour
{
    public GameObject shelf;
    [SerializeField] private bool anomalyTriggered = false;
    void Start()
    {
        
    }

    public void Levitate()
    { 
        if(!anomalyTriggered)
        {
            shelf.transform.position =  new Vector3(1.492759f,2.96355f,-67.89066f);
            shelf.transform.eulerAngles = new Vector3(0,0,180); 
        }
    }
}
