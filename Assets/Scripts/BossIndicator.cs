using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIndicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _rotatingSprite;
    [SerializeField] private  SpriteRenderer _fixedSprite;
    private Transform _boss;
    private Camera _camera;
    private float _viewWidth;
    private float _viewHeight;
    private float _offScreenMargin = 2f;
    private float _onScreenMarginX = 2.7f;
    private float _onScreenMarginY = 2f;
    private float _offScreenOffsetX;
    private float _offScreenOffsetY;
    private float _onScreenOffsetX;
    private float _onScreenOffsetY;
    private Vector3 _cameraToBoss;
    private Vector3 _iconPosition;

    private void Start ()
    {
        _boss = LD50.Scripts.AI.BossEnemyAI.Instance.transform;
        _camera = Camera.main;
        _viewHeight = 2 * _camera.orthographicSize;
        _viewWidth = _viewHeight * _camera.aspect;
        _offScreenOffsetX = _viewWidth * 0.5f + _offScreenMargin;
        _offScreenOffsetY = _viewHeight * 0.5f + _offScreenMargin;
        _onScreenOffsetX = _viewWidth * 0.5f - _onScreenMarginX;
        _onScreenOffsetY = _viewHeight * 0.5f - _onScreenMarginY;
    }

    private void Update ()
    {
        if (_boss == null)
        {
            SetVisibility(false);
            return; 
        }

        _cameraToBoss = _boss.position - _camera.transform.position;
        _cameraToBoss.z = 0f;

        // if boss is not offscreen, hide
        if (!BossOffScreen())
        {
            SetVisibility(false);
            return;
        }

        SetVisibility(true);

        // find intersection vector with camera edge + margin
        GetIconPosition();

        // Set this transform there and show relevant info
        transform.localPosition = _iconPosition;
        float directionAngle = Vector2.SignedAngle(Vector2.up, _cameraToBoss);
        _rotatingSprite.transform.eulerAngles = Vector3.forward * directionAngle;
    }

    private bool BossOffScreen ()
    {
        return (Mathf.Abs(_cameraToBoss.x) > _offScreenOffsetX ||
                Mathf.Abs(_cameraToBoss.y) > _offScreenOffsetY);
    }

    private void GetIconPosition ()
    {
        float relativeX = Mathf.Abs(_cameraToBoss.x / _onScreenOffsetX);
        float relativeY = Mathf.Abs(_cameraToBoss.y / _onScreenOffsetY);
        if (relativeX > relativeY)
        {
            _iconPosition = _cameraToBoss / relativeX;
        }
        else
        {
            _iconPosition = _cameraToBoss / relativeY;
        }
        _iconPosition.z = 5f;
    }

    private void SetVisibility(bool visible)
    {
        _fixedSprite.enabled = visible;
        _rotatingSprite.enabled = visible;
    }
}
