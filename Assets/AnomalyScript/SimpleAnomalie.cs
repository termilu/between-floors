using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnomalie : MonoBehaviour
{
    public Material[] material;
    public Material normal;
    public Material anomalyMat;
    Renderer rend;
    public bool anomaly;


    public GameObject screen;
    void Start()
    {
        rend = screen.GetComponent<Renderer>();
        material = rend.materials;
    }

    public void changeMaterial ()
    {
        if(!anomaly)
        {
            material[1] = normal;
        }   
        else
        {
            material[1] = anomalyMat;
        }  
        rend.materials = material;
    }

    public void toggleAnomaly()
    {
        anomaly = !anomaly;
    }

    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space))
        {
            toggleAnomaly();
            print(anomaly);
        } 

        if(Input.GetKeyDown(KeyCode.E))
        {
            changeMaterial();
            print("change");
        } 
    }
}
