using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    public void OnMainMenuPressed()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[EndScreenUI] GameManager missing.");
            return;
        }
        GameManager.Instance.GoToMainMenu();
    }

    public void OnRestartPressed()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[EndScreenUI] GameManager missing.");
            return;
        }
        GameManager.Instance.StartGame();
    }

    public void OnQuitPressed()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[EndScreenUI] GameManager missing.");
            return;
        }
        GameManager.Instance.QuitGame();
    }
}
