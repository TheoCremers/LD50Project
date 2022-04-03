using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Transform UpgradeContainer = null;

    public TMP_Text DistanceIndicator;


    private void Awake ()
    {
        Instance = this;
    }

    void Start()
    {
        DistanceIndicator.text = "Distance: 0";
    }

    // Update is called once per frame
    void Update()
    {
        var distanceFromCenter = Vector2.Distance(PlayerController.Instance.transform.position, Vector2.zero);
        DistanceIndicator.text = "Distance: " + distanceFromCenter.ToString();
    }

    private void OnDestroy ()
    {
        Instance = null;
    }
}
