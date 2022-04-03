using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public int currentExperience = 0;

    //List of available upgrades
    //private List<>



    public void AddExperience(int amount)
    {
        currentExperience += amount;
        // check if new upgrades become available
    }

    public void AddRangedDamage(int amount)
    {
        PlayerController.Instance.rangedAttack.damage += amount;
    }

    public void AddRangedPierce(int amount)
    {
        PlayerController.Instance.rangedAttack.piercingAmount += amount;
    }

    public void AddMeleeDamage(int amount)
    {
        PlayerController.Instance.meleeAttack.damage += amount;
    }
}
