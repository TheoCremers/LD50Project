using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeTile : MonoBehaviour
{
    public UpgradeOption UpgradeOption = null;
    public Button Button = null;
    public Image Icon = null;
    public TextMeshProUGUI CostText = null;

    private void OnDestroy ()
    {
        Button.onClick.RemoveAllListeners();
    }

    public void SetUpgradeOption(UpgradeOption upgradeOption)
    {
        UpgradeOption = upgradeOption;
        SetIconSprite(upgradeOption.upgradeImage, upgradeOption.spriteColor);
        SetPriceText(upgradeOption.expCost);
    }

    public void SetIconSprite(Sprite sprite, Color color)
    {
        Icon.sprite = sprite;
        Icon.color = color;
    }

    public void SetPriceText(int amount)
    {
        CostText.text = amount.ToString();
    }

    public void EnableButton ()
    {
        Button.interactable = true;
        CostText.color = Color.green;
    }

    public void DisableButton()
    {
        Button.interactable = false;
        CostText.color = Color.red;
    }
}
