using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WristChecklist : MonoBehaviour
{
    [Serializable]
    public class TaskRow
    {
        [Tooltip("Unique key used for saving/loading. Example: task_monitor")]
        public string id;

        [TextArea]
        public string description;

        [Header("UI References")]
        public TextMeshProUGUI text;
        public Button checkboxButton;
        public GameObject checkmarkObject; // enable/disable to show tick

        [NonSerialized] public bool completed;
    }

    [Header("Tasks")]
    public List<TaskRow> tasks = new();

    [Header("Optional")]
    public bool persistWithPlayerPrefs = false;
    public string savePrefix = "wrist_checklist_";

    private void Awake()
    {
        // Basic validation (fail loudly)
        for (int i = 0; i < tasks.Count; i++)
        {
            var t = tasks[i];
            if (string.IsNullOrWhiteSpace(t.id))
                Debug.LogError($"Checklist task at index {i} has empty id.", this);

            if (t.text == null || t.checkboxButton == null || t.checkmarkObject == null)
                Debug.LogError($"Checklist task '{t.id}' is missing UI references.", this);
        }
    }

    private void Start()
    {
        // Load + bind UI
        for (int i = 0; i < tasks.Count; i++)
        {
            int idx = i;

            if (persistWithPlayerPrefs)
                tasks[idx].completed = PlayerPrefs.GetInt(savePrefix + tasks[idx].id, 0) == 1;
            else
                tasks[idx].completed = false;

            // Set label
            if (tasks[idx].text != null)
                tasks[idx].text.text = tasks[idx].description;

            // Bind click
            if (tasks[idx].checkboxButton != null)
            {
                tasks[idx].checkboxButton.onClick.RemoveAllListeners();
                tasks[idx].checkboxButton.onClick.AddListener(() => Toggle(idx));
            }

            ApplyVisual(idx);
        }
    }

    public void Toggle(int index)
    {
        if (index < 0 || index >= tasks.Count) return;

        tasks[index].completed = !tasks[index].completed;

        if (persistWithPlayerPrefs)
            PlayerPrefs.SetInt(savePrefix + tasks[index].id, tasks[index].completed ? 1 : 0);

        ApplyVisual(index);
    }

    public void ResetAll()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].completed = false;
            if (persistWithPlayerPrefs)
                PlayerPrefs.DeleteKey(savePrefix + tasks[i].id);
            ApplyVisual(i);
        }
    }

    private void ApplyVisual(int index)
    {
        var t = tasks[index];
        if (t.checkmarkObject != null)
            t.checkmarkObject.SetActive(t.completed);

        // Optional: strike-through when completed
        if (t.text != null)
            t.text.fontStyle = t.completed ? FontStyles.Strikethrough : FontStyles.Normal;
    }
}
