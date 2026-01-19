using UnityEngine;

public class LightSwitchTrigger : MonoBehaviour
{
    [Header("Assign the lights to control")]
    public Light[] lights;

    [Header("Behavior")]
    public bool triggerOnce = true;
    public bool turnOff = true; // if false, toggles instead

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && used) return;

        used = true;

        if (lights == null || lights.Length == 0) return;

        if (turnOff)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null)
                    lights[i].enabled = false;
            }
        }
        else
        {
            // Toggle behavior
            bool anyOn = false;
            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null && lights[i].enabled)
                {
                    anyOn = true;
                    break;
                }
            }

            bool newState = !anyOn;
            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null)
                    lights[i].enabled = newState;
            }
        }
    }
}
