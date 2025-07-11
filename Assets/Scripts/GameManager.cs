using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject endPanel;

    [Header("Buttons")]
    public Button startButton;
    public Button pauseButton;
    public Button continueButton;
    public Button restartButtonPause;
    public Button restartButtonEnd;

    private bool isGameActive;
    private bool isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeButtons();
    }

    private void Start()
    {
        ShowMenu();
    }

    private void InitializeButtons()
    {
        startButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(TogglePause);
        continueButton.onClick.AddListener(ContinueGame);
        restartButtonPause.onClick.AddListener(RestartGame);
        restartButtonEnd.onClick.AddListener(RestartGame);
    }

    public void StartGame()
    {
        if (CardManager.Instance == null)
        {
            Debug.LogError("CardManager not initialized!");
            return;
        }

        isGameActive = true;
        isPaused = false;
        Time.timeScale = 1f;
        
        menuPanel.SetActive(false);
        endPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        
        CardManager.Instance.InitializeDeck();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        
        if (isPaused)
        {
            pausePanel.SetActive(true);
            Vibration.VibratePop();
        }
        else
        {
            pausePanel.SetActive(false);
            Vibration.VibratePeek();
        }
        Shake.instance.ScreenShake();
    }

    public void ContinueGame() => TogglePause();

    public void RestartGame()
    {
        Vibration.Vibrate();
        Shake.instance.ScreenShake();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        isGameActive = false;
        gamePanel.SetActive(false);
        endPanel.SetActive(true);
        Vibration.Vibrate();
        Shake.instance.ScreenShake();
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        endPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public bool IsGameActive() => isGameActive && !isPaused;
}
