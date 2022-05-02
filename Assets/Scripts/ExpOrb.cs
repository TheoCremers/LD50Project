using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int ExpValue = 10;
        
    [HideInInspector] public Vector3 Direction = Vector3.zero;
    [HideInInspector] public float Speed = 0f;

    private float _lifetime = 0f;

    private static Transform PlayerTransform = null;
    private static float MaxAcceleration = 5.0f;
    private static float SettleTime = 1f;

    private void Start ()
    {
        if (PlayerTransform == null)
        {
            PlayerTransform = PlayerController.Instance.transform;
        }
    }

    private void Update ()
    {
        if (_lifetime < SettleTime && Speed > 0f)
        {
            Speed = Speed - MaxAcceleration * Time.deltaTime;
            if (Speed < 0f) { Speed = 0f; }
        }
        else 
        {
            Vector3 relativeToPlayer = PlayerTransform.position - transform.position;
            Speed = Speed + MaxAcceleration * Time.deltaTime;

            if (relativeToPlayer.sqrMagnitude < 0.25f)
            {
                PlayerController.Instance.LevelingSystem.ChangeExperience(ExpValue);
                Destroy(gameObject);
            }
            else
            {
                Direction = relativeToPlayer.normalized;
            }
        }

        transform.position += Direction * Speed * Time.deltaTime;

        _lifetime += Time.deltaTime;
    }
}
