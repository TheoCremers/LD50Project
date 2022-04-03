using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeTile : MonoBehaviour
{
    private Button _button = null;

    public int expCost = 1000;

    public UnityEvent UpgradeEvent;

    private void Start ()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnUpgradeActivated);
    }

    private void OnUpgradeActivated ()
    {
        UpgradeEvent?.Invoke();
    }
}
