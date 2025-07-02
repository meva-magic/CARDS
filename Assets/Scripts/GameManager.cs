using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endPanel;

    [Header("References")]
    [SerializeField] private Transform cardSpawnPoint;

    public bool IsGameActive { get; private set; }
    private Keyboard keyboard;
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
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnsubscribeEvents();

    private void SubscribeEvents()
    {
        keyboard.onTextInput += HandleKeyboardInput;
    }

    private void UnsubscribeEvents()
    {
        keyboard.onTextInput -= HandleKeyboardInput;
    }

    private void HandleKeyboardInput(char ch)
    {
        if (ch == ' ' && IsGameActive)
            CardManager.Instance.DrawCard();
    }

    public void StartGame()
    {
        TogglePanels(menu: false, game: true);
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

    private void TogglePanels(bool menu, bool game, bool pause = false, bool end = false)
    {
        menuPanel.SetActive(menu);
        gamePanel.SetActive(game);
        pausePanel.SetActive(pause);
        endPanel.SetActive(end);
    }

    public void ResetGame() => SceneManager.LoadScene(currentSceneName);
}
