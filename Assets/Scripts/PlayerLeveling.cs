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
        _currentUpgradeOptions.Clear();

        UIManager.Instance.UpdateExpCounter(currentExperience);
    }

    public void ChangeExperience(int amount)
    {
        currentExperience += amount;

        // check if new upgrades become available
        foreach (var item in _upgradeTiles)
        {
            if (item.UpgradeOption.expCost <= currentExperience)
            {
                item.EnableButton();
            }
            else
            {
                item.DisableButton();
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

    public void AddSummonerLevel()
    {
        PlayerController.Instance.SummonLevel += 1;
    }

    public void ReduceSummonCooldown (float factorChange)
    {
        PlayerController.Instance.SummonCooldownFactor -= factorChange;
    }

    private UpgradeTile AddUpgradeTile (UpgradeOption option)
    {
        UpgradeTile newTile = Instantiate(_tileTemplate, UIManager.Instance.UpgradeContainer);
        _upgradeTiles.Add(newTile);
        newTile.SetUpgradeOption(option);

        newTile.Button.onClick.AddListener(() => ApplyUpgrade(option));
        newTile.Button.onClick.AddListener(() => RemoveTile(newTile));

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
            case UpgradeType.summonSpeed:
                ReduceSummonCooldown(option.floatValue);
                break;
            case UpgradeType.summonLevel:
                AddSummonerLevel();
                break;
            default:
                break;
        }

        _currentUpgradeOptions.Remove(option);
        foreach (var item in option.unlocksOptions)
        {
            _currentUpgradeOptions.Add(item);
        }

        if (_currentUpgradeOptions.Count > 0)
        {
            int index = Random.Range(0, _currentUpgradeOptions.Count);
            AddUpgradeTile(_currentUpgradeOptions[index]);
            _currentUpgradeOptions.RemoveAt(index);
        }
    }

    private void RemoveTile(UpgradeTile tile)
    {
        _upgradeTiles.Remove(tile);
        Destroy(tile.gameObject);
    }
}
