using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject endPanel;

    public bool IsGameActive { get; private set; }

    private string currentSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsGameActive)
        {
            TogglePause();
        }
    }

    private void InitializeGame()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        IsGameActive = false;
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        IsGameActive = true;
        Time.timeScale = 1f;
        CardManager.Instance.ResetDeck();
    }

    public void TogglePause()
    {
        bool shouldPause = !pausePanel.activeSelf;
        pausePanel.SetActive(shouldPause);
        Time.timeScale = shouldPause ? 0f : 1f;
    }

    public void ShowEndGame()
    {
        endPanel.SetActive(true);
        IsGameActive = false;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(currentSceneName);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(currentSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
