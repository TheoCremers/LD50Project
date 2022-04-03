using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expValue = 10;
        
    public Vector3 direction = Vector3.zero;
    public float speed = 0f;

    private float _lifetime = 0f;

    private static Transform playerTransform = null;
    private static float maxAcceleration = 5.0f;
    private static float settleTime = 1f;

    private void Start ()
    {
        if (playerTransform == null)
        {
            playerTransform = PlayerController.Instance.transform;
        }
    }

    private void Update ()
    {
        if (_lifetime < settleTime && speed > 0f)
        {
            speed = speed - maxAcceleration * Time.deltaTime;
            if (speed < 0f) { speed = 0f; }
        }
        else 
        {
            Vector3 relativeToPlayer = playerTransform.position - transform.position;
            speed = speed + maxAcceleration * Time.deltaTime;

            if (relativeToPlayer.sqrMagnitude < 0.25f)
            {
                PlayerController.Instance.levelingSystem.ChangeExperience(expValue);
                Destroy(gameObject);
            }
            else
            {
                direction = relativeToPlayer.normalized;
            }
        }

        transform.position += direction * speed * Time.deltaTime;

        _lifetime += Time.deltaTime;
    }
}
