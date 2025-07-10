using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endPanel;

    [Header("UI Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButtonPause; // Specific to pause panel
    [SerializeField] private Button menuButtonPause;    // Specific to pause panel
    [SerializeField] private Button restartButtonEnd;   // Specific to end panel
    [SerializeField] private Button menuButtonEnd;      // Specific to end panel
    [SerializeField] private Button[] categoryButtons;

    public bool IsGameActive { get; private set; }
    public bool IsPaused { get; private set; }

    private Keyboard keyboard;
    private Touchscreen touchscreen;
    private string currentSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        keyboard = Keyboard.current;
        touchscreen = Touchscreen.current;
        currentSceneName = SceneManager.GetActiveScene().name;
        
        InitializeButtons();
    }

    private void Start()
    {
        DisableAllPanels();
        menuPanel.SetActive(true);
    }

    private void InitializeButtons()
    {
        // Main Menu
        startButton.onClick.AddListener(StartGame);
        
        // Category Buttons
        foreach (Button btn in categoryButtons)
        {
            btn.onClick.AddListener(() => ToggleCategory(btn));
        }
        
        // Game Button
        pauseButton.onClick.AddListener(TogglePause);
        
        // Pause Panel Buttons
        continueButton.onClick.AddListener(ContinueGame);
        restartButtonPause.onClick.AddListener(ResetGame);
        menuButtonPause.onClick.AddListener(ReturnToMenu);
        
        // End Panel Buttons
        restartButtonEnd.onClick.AddListener(ResetGame);
        menuButtonEnd.onClick.AddListener(ReturnToMenu);
    }

    private void ToggleCategory(Button categoryButton)
    {
        Vibration.VibratePeek();
        // Your category toggle implementation
    }

    private void DisableAllPanels()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    public void StartGame()
    {
        DisableAllPanels();
        gamePanel.SetActive(true);
        IsGameActive = true;
        IsPaused = false;
        Time.timeScale = 1f;
        CardManager.Instance.ResetDeck();
        
        Vibration.VibratePeek();
        Shake.instance.CamShake();
    }

    public void ContinueGame()
    {
        DisableAllPanels();
        gamePanel.SetActive(true);
        IsPaused = false;
        Time.timeScale = 1f;
        Vibration.VibratePeek();
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        
        if (IsPaused)
        {
            DisableAllPanels();
            pausePanel.SetActive(true);
        }
        else
        {
            DisableAllPanels();
            gamePanel.SetActive(true);
        }
        
        Vibration.VibratePop();
        Shake.instance.CamShake();
    }

    public void ShowEndGame()
    {
        DisableAllPanels();
        endPanel.SetActive(true);
        IsGameActive = false;
    }

    public void ResetGame()
    {
        Vibration.Vibrate();
        Shake.instance.CamShake();
        SceneManager.LoadScene(currentSceneName);
    }

    public void ReturnToMenu()
    {
        Vibration.VibratePeek();
        SceneManager.LoadScene(currentSceneName);
    }

    private void Update()
    {
        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            if (IsGameActive) TogglePause();
            else if (menuPanel.activeSelf) ReturnToMenu();
        }
        
        if (touchscreen != null && touchscreen.touches.Count >= 3)
        {
            foreach (var touch in touchscreen.touches)
            {
                if (touch.phase.ReadValue() == TouchPhase.Began)
                {
                    TogglePause();
                    break;
                }
            }
        }
    }
}
