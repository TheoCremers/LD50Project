using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text DistanceIndicator;
    // Start is called before the first frame update
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
}
