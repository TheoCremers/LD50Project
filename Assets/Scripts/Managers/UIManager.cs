using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Transform UpgradeContainer = null;
    public TMP_Text DistanceIndicator;
    public TMP_Text ExpCounter;
    public TMP_Text PauseText;
    public TMP_Text SurvivalTime;
    public TMP_Text GameOverMessage;
    public TMP_Text RestartMessage;
    public TMP_Text Credits;

    [SerializeField] private GameObject _settingsPlaceholder = null;

    [SerializeField] private GameObject _menuOverlay = null;
    [SerializeField] private CanvasGroup _gameOverGroup = null;

    public static bool Paused = false;
    public static bool GameOver = false;

    private bool _pauseExplained = false;

    private void Awake ()
    {
        Instance = this;
        Time.timeScale = 1f;
        GameOver = false;
        Paused = false;
    }

    void Start()
    {
#if UNITY_EDITOR
        DistanceIndicator.text = "Distance: 0";
#else
        DistanceIndicator.enabled = false;
#endif
        _menuOverlay.SetActive(false);
        _settingsPlaceholder.SetActive(false);
        PauseText.enabled = false;

        InputManager.PauseEvent.AddListener(OpenLevelMenu);
        InputManager.UnpauseEvent.AddListener(CloseLevelMenu);
        InputManager.RestartEvent.AddListener(TryRestart);
        InputManager.MouseAndKeyBoardEnabled.AddListener(SetPauseTextKeyboard);
        InputManager.MouseAndKeyBoardEnabled.AddListener(SetRestartTextKeyboard);
        InputManager.GamepadEnabled.AddListener(SetPauseTextGamepad);
        InputManager.GamepadEnabled.AddListener(SetRestartTextGamepad);
    }

    private void OnDestroy ()
    {
        Instance = null;

        InputManager.PauseEvent.RemoveListener(OpenLevelMenu);
        InputManager.UnpauseEvent.RemoveListener(CloseLevelMenu);
        InputManager.RestartEvent.RemoveListener(TryRestart);
        InputManager.MouseAndKeyBoardEnabled.RemoveListener(SetPauseTextKeyboard);
        InputManager.MouseAndKeyBoardEnabled.RemoveListener(SetRestartTextKeyboard);
        InputManager.GamepadEnabled.RemoveListener(SetPauseTextGamepad);
        InputManager.GamepadEnabled.RemoveListener(SetRestartTextGamepad);
    }

    void Update ()
    {
#if UNITY_EDITOR
        if (!GameOver)
        {
            var distanceFromCenter = Vector2.Distance(PlayerController.Instance.transform.position, Vector2.zero);
            DistanceIndicator.text = $"Distance: {distanceFromCenter.ToString("0.00")}";
        }
#endif
#if !UNITY_WEBGL
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
#endif

        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreenMode = fullScreenMode;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, fullScreenMode, 60);
        }
        else if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            Screen.SetResolution(960, 540, false, 60);
        }
    }

    public void UpdateExpCounter (int amount)
    {
        ExpCounter.text = $"Exp: {amount.ToString()}";
    }

    private void ToggleLevelMenu ()
    {
        Paused = !Paused;
        if (Paused)
        {
            OpenLevelMenu();
        }
        else
        {
            CloseLevelMenu();
        }
    }

    private void OpenLevelMenu ()
    {
        if (GameOver) { return; }

        Paused = true;
        _menuOverlay.SetActive(true);
        _settingsPlaceholder.SetActive(true);
        PauseText.enabled = true;
        SetPauseText();
        Time.timeScale = 0f;
        InputManager.Instance.Input.SwitchCurrentActionMap("UI");

        _pauseExplained = true;
    }

    private void CloseLevelMenu ()
    {
        if (GameOver) { return; }

        Paused = false;
        _menuOverlay.SetActive(false);
        _settingsPlaceholder.SetActive(false);
        PauseText.enabled = false;
        Time.timeScale = 1f;

        InputManager.Instance.Input.SwitchCurrentActionMap("Player");
    }

    public void ShowPauseTip ()
    {
        if (!_pauseExplained)
        {
            PauseText.enabled = true;
            SetPauseText();
        }
    }

    public void SetPauseText ()
    {
        if (InputManager.Instance.UsingMouseAndKeyboard)
        {
            SetPauseTextKeyboard();
        }
        else
        {
            SetPauseTextGamepad();
        }
    }

    public void SetPauseTextKeyboard ()
    {
        if (!PauseText.enabled) { return; }

        if (Paused)
        {
            PauseText.text = "Press SPACE to resume";
        }
        else
        {
            PauseText.text = "Press SPACE";
        }
    }

    public void SetPauseTextGamepad() {
        if (!PauseText.enabled) { return; }

        if (Paused)
        {
            PauseText.text = "Press START to resume";
        }
        else
        {
            PauseText.text = "Press START";
        }
    }

    public void SetRestartTextKeyboard ()
    {
        RestartMessage.text = "Press R to restart";
    }

    public void SetRestartTextGamepad ()
    {
        RestartMessage.text = "Press SELECT to restart";
    }

    public void TriggerGameOver (bool victory)
    {
        Time.timeScale = 0f;
        GameOver = true;
        _gameOverGroup.alpha = 1f;
        _gameOverGroup.interactable = true;
        _gameOverGroup.blocksRaycasts = true;
        InputManager.Instance.Input.SwitchCurrentActionMap("UI");

        if (victory)
        {
            GameOverMessage.text = "VICTORY";
            GameOverMessage.color = Color.green;
            SurvivalTime.text = $"You beat the game in\n{Time.timeSinceLevelLoad.ToString("0")} seconds";
            Credits.enabled = true;
        }
        else
        {
            GameOverMessage.text = "GAME OVER";
            GameOverMessage.color = Color.red;
            SurvivalTime.text = $"Remaining Boss Health: {(LD50.Scripts.AI.BossEnemyAI.Instance.HitpointData.HealthPercentage * 100f).ToString("0.0")}%";
            Credits.enabled = false;
        }
    }

    private void TryRestart ()
    {
        if (GameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    // Temp until we have a proper settings screen. This class is not the place for this.
    public void MuteToggle(bool muted)
    {
        if (muted) 
        {
            AudioListener.volume = 0;
        } 
        else
        {
            AudioListener.volume = 1;
        }
    }
}
