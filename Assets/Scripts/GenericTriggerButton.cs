using UnityEngine;
using UnityEngine.Events;

public class GenericTriggerButton : MonoBehaviour
{
    public UnityEvent onTriggered;

    private void OnTriggerEnter(Collider other)
    {
        // I know this is horrible, but it works LOL
        if (other.transform.root.name == "Elevator")
        {
            return;
        }
        
        Debug.Log(
            "[GenericTriggerButton] TRIGGER\n" +
            "Other name: " + other.name + "\n" +
            "Other tag: " + other.tag + "\n" +
            "Other layer: " + LayerMask.LayerToName(other.gameObject.layer) + "\n" +
            "Root object: " + other.transform.root.name
        );

        onTriggered?.Invoke();
    }
}