using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowTargetWithMargin : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform = null;
    [SerializeField] [Range(0f, 1f)] private float _relativeBoundsX = 0.8f;
    [SerializeField] [Range(0f, 1f)] private float _relativeBoundsY = 0.8f;

    private Camera _camera;
    private float _maxTargetOffsetX = 0f;
    private float _maxTargetOffsetY = 0f;
    private float _cameraSizeX;
    private float _cameraSizeY;

    private void LateUpdate ()
    {
        float relativeTargetX = transform.position.x - _targetTransform.position.x;
        float relativeTargetY = transform.position.y - _targetTransform.position.y;
        Vector2 movement = Vector2.zero;

        if (relativeTargetX > _maxTargetOffsetX)
        {
            movement += Vector2.left * (relativeTargetX - _maxTargetOffsetX);
        }
        else if (relativeTargetX < -_maxTargetOffsetX)
        {
            movement += Vector2.left * (relativeTargetX + _maxTargetOffsetX);
        }

        if (relativeTargetY > _maxTargetOffsetY)
        {
            movement += Vector2.down * (relativeTargetY - _maxTargetOffsetY);
        }
        else if (relativeTargetY < -_maxTargetOffsetY)
        {
            movement += Vector2.down * (relativeTargetY + _maxTargetOffsetY);
        }

        transform.Translate(movement);
    }

    private void OnValidate ()
    {
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }
        _cameraSizeX = _camera.orthographicSize * _camera.aspect;
        _cameraSizeY = _camera.orthographicSize;

        _maxTargetOffsetX = _cameraSizeX * _relativeBoundsX;
        _maxTargetOffsetY = _cameraSizeY * _relativeBoundsY;
    }

    private void OnDrawGizmos ()
    {
        Vector2 bottomLeft = new Vector2(-_cameraSizeX * _relativeBoundsX, -_cameraSizeY * _relativeBoundsY);
        Vector2 bottomRight = new Vector2(_cameraSizeX * _relativeBoundsX, -_cameraSizeY * _relativeBoundsY);
        Vector2 topLeft = new Vector2(-_cameraSizeX * _relativeBoundsX, _cameraSizeY * _relativeBoundsY);
        Vector2 topRight = new Vector2(_cameraSizeX * _relativeBoundsX, _cameraSizeY * _relativeBoundsY);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
