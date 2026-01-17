using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;     // root object for Main Menu UI
    [SerializeField] private GameObject settingsMenuPanel; // root object for Settings UI

    void Awake()
    {
        // Safe defaults
        ShowMainMenu();
    }

    public void OnStartPressed() => GameManager.Instance.StartGame();

    public void OnQuitPressed() => GameManager.Instance.QuitGame();

    public void OnSettingsPressed() => ShowSettingsMenu();

    public void OnBackFromSettingsPressed() => ShowMainMenu();

    private void ShowMainMenu()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        if (settingsMenuPanel) settingsMenuPanel.SetActive(false);
    }

    private void ShowSettingsMenu()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (settingsMenuPanel) settingsMenuPanel.SetActive(true);
    }
}
