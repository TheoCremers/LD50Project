using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Transform UpgradeContainer = null;
    public TMP_Text DistanceIndicator;
    public TMP_Text ExpCounter;
    public TMP_Text PauseText = null;

    [SerializeField] private GameObject _menuOverlay = null;

    public bool Paused = false;

    public UnityEvent PauseEvent;
    public UnityEvent UnpauseEvent;

    private bool _pauseTipShown = false;

    private void Awake ()
    {
        Instance = this;
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
        var distanceFromCenter = Vector2.Distance(PlayerController.Instance.transform.position, Vector2.zero);
        DistanceIndicator.text = $"Distance: {distanceFromCenter.ToString()}";

        if (Input.GetButtonDown("Menu"))
        {
            ToggleLevelMenu();
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
}
