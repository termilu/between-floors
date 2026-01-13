using UnityEngine;

public class BootLoader : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance == null)
            Debug.LogError("GameManager missing in Boot scene.");
        GameManager.Instance.GoToMainMenu();
    }
}
