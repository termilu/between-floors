using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartPressed() => GameManager.Instance.StartGame();
    public void OnQuitPressed() => GameManager.Instance.QuitGame();
}
