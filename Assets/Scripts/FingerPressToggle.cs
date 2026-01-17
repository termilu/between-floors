using TMPro;
using UnityEngine;

public class FingerPressToggle : MonoBehaviour
{
    [Header("UI")]
    public GameObject checkmarkObject;
    public TextMeshProUGUI taskText;

    [Header("Debounce")]
    public float cooldownSeconds = 0.25f;

    private bool _checked;
    private float _nextAllowedTime;

    private void Reset()
    {
        // Helpful defaults
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < _nextAllowedTime) return;
        if (!other.CompareTag("FingerTip")) return;

        _nextAllowedTime = Time.time + cooldownSeconds;
        Toggle();
    }

    private void Toggle()
    {
        _checked = !_checked;

        if (checkmarkObject != null)
            checkmarkObject.SetActive(_checked);

        if (taskText != null)
            taskText.fontStyle = _checked ? FontStyles.Strikethrough : FontStyles.Normal;
    }
}
