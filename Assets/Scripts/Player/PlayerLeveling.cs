using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class PlayerLeveling : MonoBehaviour
{
    [SerializeField] private float _baseExpPerLevel = 100f;
    [SerializeField] private float _expLevelFactor = 3f;

    private float NextLevelThreshold;

    public int CurrentLevel = 0;
    public float CurrentExperience = 0;
    public int CurrentSkillPoints = 0;

    public static int UpgradesBought = 0;
    public UnityEvent PlayerLeveled = new UnityEvent();

    //List of available upgrades
    [SerializeField] private List<UpgradeOption> _currentUpgradeOptions;

    private List<UpgradeTile> _upgradeTiles = new List<UpgradeTile>();

    private void Start ()
    {
        _upgradeTiles.AddRange(UIManager.Instance.UpgradeContainer.GetComponentsInChildren<UpgradeTile>());
        for (int i = 0; i < _upgradeTiles.Count; i++)
        {
            _upgradeTiles[i].SetButtonIndex(i);
        }

        // quick shuffle
        _currentUpgradeOptions = _currentUpgradeOptions.OrderBy(x => Guid.NewGuid()).ToList();

        for (int i = 0; i < 3; i++) 
        {
            UpgradeTile setTile = SetUpgradeTile(_currentUpgradeOptions[0]);
            if (setTile == null) { break; }
            _currentUpgradeOptions.RemoveAt(0);
        }

        UpgradesBought = 0;
        NextLevelThreshold = LevelCurve(CurrentLevel + 1);
        PlayerLeveled.AddListener(OnPlayerLeveled);

        // set UI values
        UIManager.Instance.UpdateExpCounter(CurrentExperience / NextLevelThreshold, CurrentLevel);
        UIManager.Instance.UpdateSkillPointCounter(CurrentSkillPoints);
    }

    private void OnDestroy ()
    {
        PlayerLeveled.RemoveAllListeners();
    }

    public void ChangeExperience(int amount)
    {
        CurrentExperience += amount;        

        // check if any new levels are reached
        while (CurrentExperience >= NextLevelThreshold)
        {
            CurrentExperience -= NextLevelThreshold;
            CurrentLevel++;
            PlayerLeveled?.Invoke();
            ChangeSkillPoints(1);
            NextLevelThreshold = LevelCurve(CurrentLevel + 1);
        }

        UIManager.Instance.UpdateExpCounter(CurrentExperience / NextLevelThreshold, CurrentLevel);
    }

    private void ChangeSkillPoints (int amount)
    {
        CurrentSkillPoints += amount;

        // check if new upgrades become available/disabled
        bool anyUpgrade = false;
        foreach (var item in _upgradeTiles)
        {
            if (item.UpgradeOption.SkillPointCost <= CurrentSkillPoints)
            {
                item.EnableButton();
                anyUpgrade = true;
            }
            else
            {
                item.DisableButton();
            }
        }

        UIManager.Instance.UpdateSkillPointCounter(CurrentSkillPoints);
        if (anyUpgrade)
        {
            UIManager.Instance.ShowPauseTip();
        }
    }


    private float LevelCurve (int LevelToGet)
    {
        float requiredExp = _baseExpPerLevel + Mathf.Pow(LevelToGet, _expLevelFactor);
        return requiredExp;
    }

    private void OnPlayerLeveled ()
    {
        PlayerController.Instance.HitpointData.ChangeMaxHealthAndScaleCurrent(10);
        PlayerController.Instance.RangedAttack.Damage += 1;
        PlayerController.Instance.MeleeAttack.Damage += 2;
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

        if (option.SkillPointCost <= CurrentSkillPoints)
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
        UpgradeOption option = tile.UpgradeOption;
        if (CurrentSkillPoints < option.SkillPointCost) { return; }

        ChangeSkillPoints(-option.SkillPointCost);
        tile.SetInactive();

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
            case UpgradeType.meleeAoe:
                SetMeleeAoe();
                break;
            case UpgradeType.health:
                AddMaxHealth(option.intValue);
                break;
            case UpgradeType.regen:
                AddRegen(option.floatValue);
                break;
            case UpgradeType.chainLighting:
                AddChainLightning();
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
            int index = UnityEngine.Random.Range(0, _currentUpgradeOptions.Count);
            var nextUpgrade = _currentUpgradeOptions[index];
            //var nextUpgrade = _currentUpgradeOptions.OrderBy((x) => x.skillPointCost).First();
            UpgradeTile setTile = SetUpgradeTile(nextUpgrade);
            if (setTile == null) { break; } // no more blank tiles available
            _currentUpgradeOptions.Remove(nextUpgrade);
        }

        UpgradesBought++;
    }

    public void ApplyUpgrade (int tileIndex)
    {
        if (_upgradeTiles.Count > tileIndex)
        {
            ApplyUpgrade(_upgradeTiles[tileIndex]);
        }
    }

    public void AddRangedDamage (int amount)
    {
        PlayerController.Instance.RangedAttack.Damage += amount;
    }

    public void AddRangedPierce (int amount)
    {
        PlayerController.Instance.RangedAttack.PiercingAmount += amount;
    }

    public void AddMeleeDamage (int amount)
    {
        PlayerController.Instance.MeleeAttack.Damage += amount;
    }

    public void AddSummonerLevel ()
    {
        PlayerController.Instance.SummonLevel += 1;
    }

    public void AddMaxHealth(int amount)
    {
        PlayerController.Instance.HitpointData.MaxHealth += amount;
        PlayerController.Instance.HitpointData.Heal(amount * 2);
    }

    public void AddRegen(float amount)
    {
        PlayerController.Instance.HitpointData.RegenFactor = PlayerController.Instance.HitpointData.RegenFactor * amount;
    }

    public void AddChainLightning()
    {
        PlayerController.Instance.RangedAttack.ChainLightning = true;
        PlayerController.Instance.RangedAttack.PiercingAmount += 3;
        Color newColor;
        ColorUtility.TryParseHtmlString("#41A7F1", out newColor);
        PlayerController.Instance.RangedAttack.BulletColor = newColor;
    }

    public void ReduceSummonCooldown (float factorChange)
    {
        PlayerController.Instance.SummonCooldownFactor -= factorChange;        
    }

    public void SetMeleeAoe ()
    {
        PlayerController.Instance.MeleeAttack.LeavesAoe = true;
    }
}
