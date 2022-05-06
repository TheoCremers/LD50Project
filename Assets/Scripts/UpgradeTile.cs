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

    private void Start ()
    {
        UIManager.Instance.PauseEvent.AddListener(ShowDetails);
        UIManager.Instance.UnpauseEvent.AddListener(HideDetails);
        Details.enabled = false;
    }

    private void OnDestroy ()
    {
        Button.onClick.RemoveAllListeners();
        UIManager.Instance?.PauseEvent.RemoveListener(ShowDetails);
        UIManager.Instance?.UnpauseEvent.RemoveListener(HideDetails);
    }

    public void SetUpgradeOption(UpgradeOption upgradeOption)
    {
        UpgradeOption = upgradeOption;
        SetIconSprite(upgradeOption.upgradeImage, upgradeOption.spriteColor);
        SetPriceText(upgradeOption.expCost);
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

    public void SetDetailsText(string details)
    {
        Details.text = details;
    }

    public void ShowDetails()
    {
        Details.enabled = true;
    }

    public void HideDetails ()
    {
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
}
