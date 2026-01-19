using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum ElevatorChoice { Up, Down }

    [Header("Scenes")]
    public string mainMenuScene = "MainMenu";
    public string gameScene = "OfficeScene";

    [Header("Round / Win Settings")]
    [Range(0f, 1f)] public float anomalyChance = 0.5f;
    public int winAfterCorrect = 5;

    [Header("Optional End Scenes (leave blank to return to MainMenu)")]
    public string winScene = "";
    public string loseScene = "";

    public bool IsPaused { get; private set; }

    public bool RoundHasAnomaly { get; private set; }
    public bool HasRound { get; private set; }
    public int CorrectCount { get; private set; }

    // Elevator selection state
    public bool HasPendingElevatorChoice { get; private set; }
    private ElevatorChoice pendingChoice;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        CorrectCount = 0;
        HasPendingElevatorChoice = false;

        GenerateNextRound();
        SceneManager.LoadScene(gameScene);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        HasRound = false;
        CorrectCount = 0;
        HasPendingElevatorChoice = false;

        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame() => Application.Quit();

    public void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    public void GenerateNextRound()
    {
        RoundHasAnomaly = Random.value < anomalyChance;
        HasRound = true;
    }

    // Called by UP/DOWN buttons
    public void SetPendingElevatorChoice(ElevatorChoice choice)
    {
        pendingChoice = choice;
        HasPendingElevatorChoice = true;
        Debug.Log($"[GameManager] Pending elevator choice set: {choice}");
    }

    // Called when player walks into elevator cabin trigger
    public void CommitPendingChoiceAndLoadNextRound()
    {
        if (!HasPendingElevatorChoice)
        {
            Debug.Log("[GameManager] Player entered elevator but no choice was selected.");
            return;
        }

        bool playerSaysAnomaly = (pendingChoice == ElevatorChoice.Down);
        bool correct = (playerSaysAnomaly == RoundHasAnomaly);

        HasPendingElevatorChoice = false;

        if (!correct)
        {
            Lose();
            return;
        }

        CorrectCount++;

        if (CorrectCount >= winAfterCorrect)
        {
            Win();
            return;
        }

        GenerateNextRound();
        SceneManager.LoadScene(gameScene);
    }

    private void Win()
    {
        if (!string.IsNullOrEmpty(winScene))
            SceneManager.LoadScene(winScene);
        else
            GoToMainMenu();
    }

    private void Lose()
    {
        if (!string.IsNullOrEmpty(loseScene))
            SceneManager.LoadScene(loseScene);
        else
            GoToMainMenu();
    }
}
