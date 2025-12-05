using UnityEngine;
using UnityEngine.Events;

public class GenericTriggerButton : MonoBehaviour
{
    public UnityEvent onTriggered;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[GenericTriggerButton] TRIGGER from: " + other.name);
        onTriggered?.Invoke();
    }
}