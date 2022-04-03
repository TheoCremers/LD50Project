using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    public GameObject[] Familiars;

    public SummoningCircle SummoningCircle;

    public void Summon (int level)
    {
        if (Familiars[level] == null) { return; }

        // Summon cap. Higher cap for higher level units, making more high level summons appear once you're near the cap.
        if (UnitManager.FriendlyUnits.Count > 20 + (level * 3)) { return; }

        var summoningCircle = Instantiate(SummoningCircle);
        summoningCircle.transform.position = PlayerController.Instance.transform.position;
        summoningCircle.Familiar = Familiars[level];    
    }
}
