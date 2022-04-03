using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Transform UpgradeContainer = null;

    public TMP_Text DistanceIndicator;
    public TMP_Text ExpCounter;


    private void Awake ()
    {
        Instance = this;
    }

    void Start()
    {
        DistanceIndicator.text = "Distance: 0";
        foreach (Transform child in UpgradeContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var distanceFromCenter = Vector2.Distance(PlayerController.Instance.transform.position, Vector2.zero);
        DistanceIndicator.text = $"Distance: {distanceFromCenter.ToString()}";
    }

    public void UpdateExpCounter (int amount)
    {
        ExpCounter.text = $"Exp: {amount.ToString()}";
    }

    private void OnDestroy ()
    {
        Instance = null;
    }
}
