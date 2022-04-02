using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    protected Transform _player;

    public TMP_Text DistanceIndicator;
    // Start is called before the first frame update
    void Start()
    {
        _player = PlayerController.Instance?.transform;

        DistanceIndicator.text = "Distance: 0";
    }

    // Update is called once per frame
    void Update()
    {
        var distanceFromCenter = Vector2.Distance(_player.position, Vector2.zero);
        DistanceIndicator.text = "Distance: " + distanceFromCenter.ToString();
    }
}
