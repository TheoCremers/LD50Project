using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLeveling : MonoBehaviour
{
    public int currentExperience = 0;

    //List of available upgrades
    [SerializeField] private List<UpgradeOption> _currentUpgradeOptions;
    [SerializeField] private UpgradeTile _tileTemplate = null;

    private List<UpgradeTile> _upgradeTiles = new List<UpgradeTile>();

    private void Start ()
    {
        foreach (var item in _currentUpgradeOptions)
        {
            AddUpgradeTile(item);
        }

        UIManager.Instance.UpdateExpCounter(currentExperience);
    }

    public void ChangeExperience(int amount)
    {
        currentExperience += amount;

        // check if new upgrades become available
        for (int i = 0; i < _currentUpgradeOptions.Count; i++)
        {
            if (_currentUpgradeOptions[i].expCost <= currentExperience)
            {
                _upgradeTiles[i].EnableButton();
            }
            else
            {
                _upgradeTiles[i].DisableButton();
            }
        }
        UIManager.Instance.UpdateExpCounter(currentExperience);
    }

    public void AddRangedDamage(int amount)
    {
        PlayerController.Instance.RangedAttack.damage += amount;
    }

    public void AddRangedPierce(int amount)
    {
        PlayerController.Instance.RangedAttack.piercingAmount += amount;
    }

    public void AddMeleeDamage(int amount)
    {
        PlayerController.Instance.MeleeAttack.damage += amount;
    }

    private UpgradeTile AddUpgradeTile (UpgradeOption option)
    {
        UpgradeTile newTile = Instantiate(_tileTemplate, UIManager.Instance.UpgradeContainer);
        _upgradeTiles.Add(newTile);
        newTile.SetIconSprite(option.upgradeImage, option.spriteColor);
        newTile.SetPriceText(option.expCost);

        newTile.button.onClick.AddListener(() => ApplyUpgrade(option));
        newTile.button.onClick.AddListener(() => RemoveTile(newTile));

        if (option.expCost <= currentExperience)
        {
            newTile.EnableButton();
        }
        else
        {
            newTile.DisableButton();
        }
        return newTile;
    }

    private void ApplyUpgrade(UpgradeOption option)
    {
        ChangeExperience(-option.expCost);

        switch (option.type)
        {
            case UpgradeType.none:
                break;
            case UpgradeType.rangedDamage:
                AddRangedDamage(option.intValue);
                break;
            case UpgradeType.meleeDamage:
                AddMeleeDamage(option.intValue);
                break;
            case UpgradeType.rangedPierce:
                AddRangedPierce(option.intValue);
                break;
        }

        int upgradeIndex = _currentUpgradeOptions.IndexOf(option);
        _currentUpgradeOptions.Remove(option);

        foreach (var item in option.unlocksOptions)
        {
            _currentUpgradeOptions.Insert(upgradeIndex, item);
            AddUpgradeTile(item);
        }
    }

    private void RemoveTile(UpgradeTile tile)
    {
        _upgradeTiles.Remove(tile);
        Destroy(tile.gameObject);
    }
}
