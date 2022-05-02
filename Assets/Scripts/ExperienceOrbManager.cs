using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrbManager : MonoBehaviour
{
    public static ExperienceOrbManager Instance;

    [SerializeField] private ExpOrb _smallExpOrb = null;
    [SerializeField] private ExpOrb _mediumExpOrb = null;
    [SerializeField] private ExpOrb _largeExpOrb = null;

    private float _minVelocity = 3f;
    private float _maxVelocity = 7f;

    private void Awake ()
    {
        Instance = this;
    }

    private void OnDestroy ()
    {
        Instance = null;
    }

    public void SpawnExperienceOrbs(Vector3 position, int value)
    {
        CreateOrbs(_largeExpOrb, position, ref value);
        CreateOrbs(_mediumExpOrb, position, ref value);
        CreateOrbs(_smallExpOrb, position, ref value);
    }

    private void CreateOrbs(ExpOrb orb, Vector3 position, ref int value)
    {
        int spawnAmount = value / orb.ExpValue;
        for (int i = 0; i < spawnAmount; i++)
        {
            ExpOrb newOrb = Instantiate(orb, transform);
            newOrb.transform.position = position;
            float randomAngle = Random.Range(0, Mathf.PI * 2f);
            newOrb.Direction = new Vector3(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle), 0f);
            newOrb.Speed = Random.Range(_minVelocity, _maxVelocity);
            value -= orb.ExpValue;
        }
    }
}
