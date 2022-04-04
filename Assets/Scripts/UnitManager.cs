using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static List<Transform> FriendlyUnits;

    public static List<Transform> EnemyUnits;

    [SerializeField]
    private float _difficultyModifier = 1f;

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
        var difficulty = Vector2.Distance(playerPosition, Vector2.zero) * _difficultyModifier;
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
            // TODO: tweaken, beetje meer randomness
            if (difficulty < 10) 
            {
                Instantiate(Enemies[0], spawnPosition, PlayerController.Instance.transform.rotation);
            } 
            else if (difficulty < 20) 
            {
                Instantiate(Enemies[1], spawnPosition, PlayerController.Instance.transform.rotation);
            }
            else
            {
                Instantiate(Enemies[2], spawnPosition, PlayerController.Instance.transform.rotation);
            }
        }
        //var spawnLocation = Vector2()
    }
}
