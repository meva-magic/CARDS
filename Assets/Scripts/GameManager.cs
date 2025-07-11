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
        Time.timeScale = 1f;
    }

    private void InitializeButtons()
    {
        startButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        restartButtonPause.onClick.RemoveAllListeners();
        restartButtonEnd.onClick.RemoveAllListeners();

        startButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(PauseGame);
        continueButton.onClick.AddListener(ContinueGame);
        restartButtonPause.onClick.AddListener(RestartGame);
        restartButtonEnd.onClick.AddListener(RestartGame);
    }

    public void StartGame()
    {
        if (CardManager.Instance != null && !CardManager.Instance.HasEnabledCategories())
        {
            EndGame();
            return;
        }

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
        
        Shake.instance.ScreenShake();
    }

    public void PauseGame()
    {
        if (!isGameActive) return;

        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        Shake.instance.ScreenShake();
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        Shake.instance.ScreenShake();
    }

    public void RestartGame()
    {
        Shake.instance.ScreenShake();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        isGameActive = false;
        gamePanel.SetActive(false);
        endPanel.SetActive(true);
        Shake.instance.ScreenShake();
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
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
