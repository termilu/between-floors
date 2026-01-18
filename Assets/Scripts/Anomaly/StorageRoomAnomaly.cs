using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageRoomAnomaly : MonoBehaviour
{
    public GameObject ghost;
    private bool anomalyTriggered = false;
    void Start()
    {
        ghost.SetActive(false);
    }

    public void Appear()
    {
        if(!anomalyTriggered)
        {
            ghost.SetActive(true);
        }
    }
}
