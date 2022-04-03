using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeTile : MonoBehaviour
{
    public Button button = null;
    public Image icon = null;
    public TextMeshProUGUI costText = null;

    private void OnDestroy ()
    {
        button.onClick.RemoveAllListeners();
    }

    public void SetIconSprite(Sprite sprite, Color color)
    {
        icon.sprite = sprite;
        icon.color = color;
    }

    public void SetPriceText(int amount)
    {
        costText.text = amount.ToString();
    }

    public void EnableButton ()
    {
        button.interactable = true;
        costText.color = Color.green;
    }

    public void DisableButton()
    {
        button.interactable = false;
        costText.color = Color.red;
    }
}
