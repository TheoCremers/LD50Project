using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

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

    // Update is called once per frame
    void Update()
    {
        if (GameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
#if UNITY_EDITOR
            var distanceFromCenter = Vector2.Distance(PlayerController.Instance.transform.position, Vector2.zero);
            DistanceIndicator.text = $"Distance: {distanceFromCenter.ToString("0.00")}";
#endif
            if (Input.GetButtonDown("Menu"))
            {
                ToggleLevelMenu();
            }
        }
    }

    public void UpdateExpCounter (int amount)
    {
        ExpCounter.text = $"Exp: {amount.ToString()}";
    }

    private void OnDestroy ()
    {
        Instance = null;
        PauseEvent.RemoveAllListeners();
        UnpauseEvent.RemoveAllListeners();
    }

    private void ToggleLevelMenu ()
    {
        Paused = !Paused;

        if (Paused)
        {
            _menuOverlay.SetActive(true);
            PauseText.text = "Press 'space' to resume";
            PauseText.enabled = true;
            Time.timeScale = 0f;
            PauseEvent?.Invoke();
        }
        else
        {
            _menuOverlay.SetActive(false);
            PauseText.enabled = false;
            Time.timeScale = 1f;
            UnpauseEvent?.Invoke();
        }
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
            GameOverMessage.color = Color.green;
            SurvivalTime.text = $"Remaining Boss Health: {(LD50.Scripts.AI.BossEnemyAI.Instance.HitpointData.HealthPercentage * 100f).ToString("0.0")}%";
            Credits.enabled = false;
        }
    }
}
