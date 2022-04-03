using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLeveling : MonoBehaviour
{
    public int currentExperience = 0;

    //List of available upgrades
    [SerializeField] private List<UpgradeOption> _currentUpgradeOptions;

    private List<UpgradeTile> _upgradeTiles = new List<UpgradeTile>();

    private void Start ()
    {
        _upgradeTiles.AddRange(UIManager.Instance.UpgradeContainer.GetComponentsInChildren<UpgradeTile>());

        while (_currentUpgradeOptions.Count > 0)
        {
            UpgradeTile setTile = SetUpgradeTile(_currentUpgradeOptions[0]);
            if (setTile == null) { break; }
            _currentUpgradeOptions.RemoveAt(0);
        }

        UIManager.Instance.UpdateExpCounter(currentExperience);
    }

    public void ChangeExperience(int amount)
    {
        currentExperience += amount;

        bool anyUpgrade = false;

        // check if new upgrades become available
        foreach (var item in _upgradeTiles)
        {
            if (item.UpgradeOption.expCost <= currentExperience)
            {
                item.EnableButton();
                anyUpgrade = true;
            }
            else
            {
                item.DisableButton();
            }
        }
        UIManager.Instance.UpdateExpCounter(currentExperience);

        if (anyUpgrade)
        {
            UIManager.Instance.ShowPauseTip();
        }
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

    public void AddSummonerLevel()
    {
        PlayerController.Instance.SummonLevel += 1;
    }

    public void ReduceSummonCooldown (float factorChange)
    {
        PlayerController.Instance.SummonCooldownFactor -= factorChange;
    }

    private UpgradeTile SetUpgradeTile (UpgradeOption option)
    {
        //UpgradeTile blankTile = Instantiate(_tileTemplate, UIManager.Instance.UpgradeContainer);
        UpgradeTile blankTile = null;
        foreach (var item in _upgradeTiles)
        {
            if (!item.Active)
            {
                blankTile = item;
                break;
            }
        }
        if (blankTile == null) { return blankTile; }

        blankTile.SetUpgradeOption(option);
        blankTile.Button.onClick.AddListener(() => ApplyUpgrade(blankTile));
        blankTile.SetActive();

        if (option.expCost <= currentExperience)
        {
            blankTile.EnableButton();
        }
        else
        {
            blankTile.DisableButton();
        }

        return blankTile;
    }

    private void ApplyUpgrade(UpgradeTile tile)
    {
        tile.SetInactive();

        UpgradeOption option = tile.UpgradeOption;

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
            case UpgradeType.summonSpeed:
                ReduceSummonCooldown(option.floatValue);
                break;
            case UpgradeType.summonLevel:
                AddSummonerLevel();
                break;
            default:
                break;
        }

        foreach (var item in option.unlocksOptions)
        {
            _currentUpgradeOptions.Add(item);
        }

        while (_currentUpgradeOptions.Count > 0)
        {
            int index = Random.Range(0, _currentUpgradeOptions.Count);
            UpgradeTile setTile = SetUpgradeTile(_currentUpgradeOptions[index]);
            if (setTile == null) { break; } // no more blank tiles available
            _currentUpgradeOptions.RemoveAt(index);
        }
    }
}
