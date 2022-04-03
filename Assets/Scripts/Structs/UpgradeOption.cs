using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeOption", menuName = "My Assets/UpgradeOption")]
public class UpgradeOption : ScriptableObject
{
    public UpgradeType type = UpgradeType.none;
    public int expCost = 100;
    public int intValue = 0;
    public float floatValue = 0f;
    public Sprite upgradeImage = null;
    public List<UpgradeOption> unlocksOptions;
}
