using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static List<Transform> FriendlyUnits;

    public static List<Transform> EnemyUnits;

    //[SerializeField]
    //private float _difficultyModifier = 0.33f;

    //[SerializeField]
    //private float _highestDifficulty = 1f;

    [SerializeField]
    private float _spawnFrequencyModifier = 1f;

    [SerializeField]
    private float _baseSpawnFrequency = 5f;

    [SerializeField]
    private int _enemyCap = 30;
    public int EnemyCount= 0;

    private float _spawnTimer = 0f;

    public GameObject[] Enemies;
 
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

    public static Transform GetClosestEnemy (Vector3 position, params Transform[] excludeTransforms)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;

        foreach (Transform t in EnemyUnits)
        {
            bool excluded = false;
            foreach (var item in excludeTransforms)
            {
                if (t == item) 
                { 
                    excluded = true;
                    break;
                }
            }
            if (excluded) { continue; }

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
        EnemyCount = EnemyUnits.Count;
        // Spawn enemy when it's time to do so
        if (_spawnTimer <= 0)
        {
            if (EnemyUnits.Count < _enemyCap) 
            {
                SpawnEnemy();
            }
            _spawnTimer = Random.Range(_baseSpawnFrequency * _spawnFrequencyModifier, _baseSpawnFrequency * _spawnFrequencyModifier * 1.5f);
        } 
        else 
        {
            _spawnTimer -= Time.deltaTime;
        }
    }

    private void SpawnEnemy() 
    {
        var playerPosition = PlayerController.Instance.transform.position;
        //var difficulty = Vector2.Distance(playerPosition, Vector2.zero) * _difficultyModifier;
        // if (difficulty > _highestDifficulty) 
        // {
        //     _highestDifficulty = difficulty;
        // }        
        var camera = Camera.main;
        
        var spawnPosition = (Vector2)playerPosition + new Vector2(Random.Range(-18f, 18f), Random.Range(-14f, 14f));

        var vpPos = Camera.main.WorldToViewportPoint(spawnPosition);
        if (vpPos.x >= 0f && vpPos.x <= 1f && vpPos.y >= 0f && vpPos.y <= 1f && vpPos.z > 0f) 
        {
            // Pick a new position
            SpawnEnemy();
        } 
        else 
        {
            if (PlayerLeveling.UpgradesBought <= 1) //if (_highestDifficulty < 15) 
            {
                Instantiate(Enemies[Random.Range(0,2)], spawnPosition, PlayerController.Instance.transform.rotation);
            } 
            else if (PlayerLeveling.UpgradesBought <= 4) //else if (_highestDifficulty < 30) 
            {
                Instantiate(Enemies[Random.Range(0,3)], spawnPosition, PlayerController.Instance.transform.rotation);
            }
            else if (PlayerLeveling.UpgradesBought <= 7) //else if (_highestDifficulty < 45) 
            {
                Instantiate(Enemies[Random.Range(1,4)], spawnPosition, PlayerController.Instance.transform.rotation);
            }
            else if (PlayerLeveling.UpgradesBought <= 12) //else if (_highestDifficulty < 60) 
            {
                Instantiate(Enemies[Random.Range(2,5)], spawnPosition, PlayerController.Instance.transform.rotation);
            }
            else if (PlayerLeveling.UpgradesBought <= 19) //else if (_highestDifficulty < 75) 
            {
                Instantiate(Enemies[Random.Range(3,6)], spawnPosition, PlayerController.Instance.transform.rotation);
            }
            else 
            {
                Instantiate(Enemies[Random.Range(4,7)], spawnPosition, PlayerController.Instance.transform.rotation);
            }
        }
    }
}
