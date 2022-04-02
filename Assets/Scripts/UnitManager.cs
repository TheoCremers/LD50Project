using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static List<Transform> FriendlyUnits;
    public static List<Transform> EnemyUnits;
 
    // Start is called before the first frame update
    void Awake()
    {
        FriendlyUnits = new List<Transform>();
        EnemyUnits = new List<Transform>();
    }

    public static Transform GetClosestEnemy(Vector3 position)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;

        foreach (Transform t in EnemyUnits)
        {
            float dist = Vector2.SqrMagnitude(position - t.position);
            //float dist = Vector3.Distance(t.position, position);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    public static Transform GetClosestFriendly(Vector3 position)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;

        foreach (Transform t in FriendlyUnits)
        {
            float dist = Vector2.SqrMagnitude(position - t.position);
            //float dist = Vector3.Distance(t.position, position);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
