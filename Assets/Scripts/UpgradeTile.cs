using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeTile : MonoBehaviour
{
    public UpgradeOption UpgradeOption = null;
    public Button Button = null;
    public Image Icon = null;
    public TextMeshProUGUI Cost = null;
    public TextMeshProUGUI Details = null;
    public bool Active = false;
    private int _index = -1;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private TextMeshProUGUI _buttonKey;
    [SerializeField] private Transform _skillPointContainer;

    private void Start ()
    {
        InputManager.PauseEvent.AddListener(ShowDetails);
        InputManager.UnpauseEvent.AddListener(HideDetails);
        InputManager.MouseAndKeyBoardEnabled.AddListener(HideGamePadButton);
        InputManager.GamepadEnabled.AddListener(ShowGamePadButton);
        Details.enabled = false;
    }

    private void OnDestroy ()
    {
        Button.onClick.RemoveAllListeners();
        InputManager.PauseEvent.RemoveListener(ShowDetails);
        InputManager.UnpauseEvent.RemoveListener(HideDetails);
        InputManager.MouseAndKeyBoardEnabled.RemoveListener(HideGamePadButton);
        InputManager.GamepadEnabled.RemoveListener(ShowGamePadButton);
    }

    public void SetUpgradeOption(UpgradeOption upgradeOption)
    {
        UpgradeOption = upgradeOption;
        SetIconSprite(upgradeOption.upgradeImage, upgradeOption.spriteColor);
        //SetPriceText(upgradeOption.expCost);
        SetSkillPointCost(upgradeOption.SkillPointCost);
        SetDetailsText(upgradeOption.details);
    }

    public void SetIconSprite(Sprite sprite, Color color)
    {
        Icon.sprite = sprite;
        Icon.color = color;
    }

    public void SetPriceText(int amount)
    {
        Cost.text = amount.ToString();
    }

    public void SetSkillPointCost(int amount)
    {
        if (_skillPointContainer.childCount < amount)
        {
            Transform skillPointIcon = _skillPointContainer.GetChild(0);
            while (_skillPointContainer.childCount < amount)
            {
                Instantiate(skillPointIcon, _skillPointContainer);
            }
        }
        for (int i = 0; i < _skillPointContainer.childCount; i++)
        {
            _skillPointContainer.GetChild(i).gameObject.SetActive(i < amount);
        }
    }

    public void SetDetailsText(string details)
    {
        Details.text = details;
    }

    public void ShowDetails()
    {
        if (UIManager.GameOver) { return; }
        Details.enabled = true;
    }

    public void HideDetails ()
    {
        if (UIManager.GameOver) { return; }
        Details.enabled = false;
    }

    public void EnableButton ()
    {
        Button.interactable = true;
        Cost.color = Color.green;
    }

    public void DisableButton()
    {
        Button.interactable = false;
        Cost.color = Color.red;
    }

    public void SetInactive ()
    {
        Active = false;
        Button.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
        Active = true;
    }

    public void SetButtonIndex(int index)
    {
        _index = index;

        switch(index)
        {
            case 0:
                _buttonImage.color = new Color(0.9f, 0.7f, 0f);
                _buttonKey.text = "Y";
                break;
            case 1:
                _buttonImage.color = new Color(0f, 0.6f, 1f);
                _buttonKey.text = "X";
                break;
            case 2:
                _buttonImage.color = new Color(0.4f, 0.8f, 0.3f);
                _buttonKey.text = "A";
                break;
        }
    }

    private void HideGamePadButton ()
    {
        _buttonImage.gameObject.SetActive(false);
    }

    private void ShowGamePadButton ()
    {
        _buttonImage.gameObject.SetActive(true);
    }
}
