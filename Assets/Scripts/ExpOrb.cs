using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expValue = 10;

    
    private Vector3 _velocity = Vector3.zero;
    private float _speed = 0f;

    private static Transform playerTransform = null;
    private static float maxAcceleration = 5.0f;

    private void Start ()
    {
        if (playerTransform == null)
        {
            playerTransform = PlayerController.Instance.transform;
        }
    }

    private void Update ()
    {
        Vector3 relativeToPlayer = playerTransform.position - transform.position;
        _speed = _speed + maxAcceleration * Time.deltaTime;

        if (relativeToPlayer.sqrMagnitude < 0.25f) 
        {
            PlayerController.Instance.LevelingSystem.ChangeExperience(expValue);
            Destroy(gameObject);
        }
        else
        {
            _velocity = relativeToPlayer.normalized * _speed;
            transform.position += _velocity * Time.deltaTime;
        }
    }
}
