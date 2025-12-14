using UnityEngine;
using UnityEngine.Events;

public class GenericTriggerButton : MonoBehaviour
{
    public string requiredTag = "Player";
    public UnityEvent onTriggered;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGERED BY: " + other.name);
        onTriggered?.Invoke();
    }

}