using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetSmooth : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform = null;
    [SerializeField] private float _smoothSpeed = 5f;

    private void FixedUpdate ()
    {
        if (_targetTransform != null)
        {
            Vector3 smoothPosition = Vector2.Lerp(transform.position, _targetTransform.position, _smoothSpeed * Time.deltaTime);
            transform.position = smoothPosition + Vector3.back * 10f; // add camera offset}
        }
    }
}
