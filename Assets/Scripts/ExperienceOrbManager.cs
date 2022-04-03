using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrbManager : MonoBehaviour
{
    public static ExperienceOrbManager Instance;

    [SerializeField] private ExpOrb smallExpOrb = null;
    [SerializeField] private ExpOrb mediumExpOrb = null;
    [SerializeField] private ExpOrb largeExpOrb = null;

    private int smallAmount;
    private int mediumAmount;
    private int largeAmount;

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

    private void Start ()
    {
        smallAmount = smallExpOrb.expValue;
        mediumAmount = mediumExpOrb.expValue;
        largeAmount = largeExpOrb.expValue;
    }

    public void SpawnExperienceOrbs(Vector3 position, int value)
    {
        CreateOrbs(largeExpOrb, position, ref value);
        CreateOrbs(mediumExpOrb, position, ref value);
        CreateOrbs(smallExpOrb, position, ref value);
    }

    private void CreateOrbs(ExpOrb orb, Vector3 position, ref int value)
    {
        int spawnAmount = value / orb.expValue;
        for (int i = 0; i < spawnAmount; i++)
        {
            ExpOrb newOrb = Instantiate(orb, transform);
            newOrb.transform.position = position;
            float randomAngle = Random.Range(0, Mathf.PI * 2f);
            newOrb.direction = new Vector3(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle), 0f);
            newOrb.speed = Random.Range(_minVelocity, _maxVelocity);
            value -= orb.expValue;
        }
    }
}
