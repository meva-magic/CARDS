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
        // Singleton pattern
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
        Time.timeScale = 1f; // Ensure game starts at normal time scale
    }

    private void InitializeButtons()
    {
        // Clear any existing listeners to prevent duplicates
        startButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        restartButtonPause.onClick.RemoveAllListeners();
        restartButtonEnd.onClick.RemoveAllListeners();

        // Add new listeners
        startButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(PauseGame);
        continueButton.onClick.AddListener(ContinueGame);
        restartButtonPause.onClick.AddListener(RestartGame);
        restartButtonEnd.onClick.AddListener(RestartGame);
    }

    public void StartGame()
    {
        isGameActive = true;
        isPaused = false;
        Time.timeScale = 1f;
        
        menuPanel.SetActive(false);
        endPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        
        if (CardManager.Instance != null)
        {
            CardManager.Instance.ResetDeck();
        }
        
        Vibration.VibratePeek();
    }

    public void PauseGame()
    {
        if (!isGameActive) return;

        isPaused = true;
        Time.timeScale = 0f;
        
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
        
        Vibration.VibratePop();
        if (Shake.instance != null)
        {
            Shake.instance.ScreenShake();
        }
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        
        Vibration.VibratePeek();
        if (Shake.instance != null)
        {
            Shake.instance.ScreenShake();
        }
    }

    public void RestartGame()
    {
        Vibration.Vibrate();
        if (Shake.instance != null)
        {
            Shake.instance.ScreenShake();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        isGameActive = false;
        gamePanel.SetActive(false);
        endPanel.SetActive(true);
        
        Vibration.Vibrate();
        if (Shake.instance != null)
        {
            Shake.instance.ScreenShake();
        }
    }

    public void ShowMenu()
    {
        isGameActive = false;
        isPaused = false;
        Time.timeScale = 1f;
        
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        endPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public bool IsGameActive() => isGameActive && !isPaused;

    private void OnDestroy()
    {
        // Clean up static instance when destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
