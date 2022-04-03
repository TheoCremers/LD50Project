using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Transform UpgradeContainer = null;
    public TMP_Text DistanceIndicator;
    public TMP_Text ExpCounter;

    [SerializeField] private GameObject _menuOverlay = null;
    [SerializeField] private TMP_Text _pauseText = null;

    public bool Paused = false;

    public UnityEvent PauseEvent;
    public UnityEvent UnpauseEvent;

    private void Awake ()
    {
        Instance = this;
    }

    void Start()
    {
        DistanceIndicator.text = "Distance: 0";

        _menuOverlay.SetActive(false);
        _pauseText.enabled = false;
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
            _pauseText.text = "Press 'space' to resume";
            _pauseText.enabled = true;
            Time.timeScale = 0f;
            PauseEvent?.Invoke();
        }
        else
        {
            _menuOverlay.SetActive(false);
            _pauseText.enabled = false;
            Time.timeScale = 1f;
            UnpauseEvent?.Invoke();
        }
    }
}
