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
    public TMP_Text Credits;

    [SerializeField] private GameObject _menuOverlay = null;
    [SerializeField] private CanvasGroup _gameOverGroup = null;

    public static bool Paused = false;
    public static bool GameOver = false;

    public UnityEvent PauseEvent;
    public UnityEvent UnpauseEvent;

    private bool _pauseTipShown = false;

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
        PauseText.enabled = false;
    }

    void Update()
    {
        if (GameOver)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
#if UNITY_EDITOR
        else
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

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreenMode = fullScreenMode;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, fullScreenMode, 60);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Screen.SetResolution(960, 540, false, 60);
        }
    }

    private void OnDestroy ()
    {
        Instance = null;
        PauseEvent.RemoveAllListeners();
        UnpauseEvent.RemoveAllListeners();
    }

    public void UpdateExpCounter (int amount)
    {
        ExpCounter.text = $"Exp: {amount.ToString()}";
    }

    public void OpenLevelMenu ()
    {
        Paused = true;
        _menuOverlay.SetActive(true);
        PauseText.text = "Press 'space' to resume";
        PauseText.enabled = true;
        Time.timeScale = 0f;
        PauseEvent?.Invoke();
    }

    public void CloseLevelMenu ()
    {
        Paused = false;
        _menuOverlay.SetActive(false);
        PauseText.enabled = false;
        Time.timeScale = 1f;
        UnpauseEvent?.Invoke();
    }

    public void ShowPauseTip ()
    {
        if (!_pauseTipShown)
        {
            PauseText.text = "Press 'space'";
            PauseText.enabled = true;
            _pauseTipShown = true;
        }
    }

    public void TriggerGameOver (bool victory)
    {
        Time.timeScale = 0f;
        GameOver = true;
        _gameOverGroup.alpha = 1f;
        _gameOverGroup.interactable = true;
        _gameOverGroup.blocksRaycasts = true;

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
}
