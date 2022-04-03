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
    public TMP_Text survivalTime;

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
        DistanceIndicator.text = "Distance: 0";

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
            var distanceFromCenter = Vector2.Distance(PlayerController.Instance.transform.position, Vector2.zero);
            DistanceIndicator.text = $"Distance: {distanceFromCenter.ToString()}";

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
            PauseText.text = "Press 'space' to view upgrade details";
            PauseText.enabled = true;
            _pauseTipShown = true;
        }
    }

    public void TriggerGameOver ()
    {
        Time.timeScale = 0f;
        GameOver = true;
        _gameOverGroup.alpha = 1f;
        _gameOverGroup.interactable = true;
        _gameOverGroup.blocksRaycasts = true;
        survivalTime.text = $"survived for\n{Time.timeSinceLevelLoad.ToString("0")} seconds";
    }
}
