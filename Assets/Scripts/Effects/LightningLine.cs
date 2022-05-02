using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningLine : MonoBehaviour
{
    public Transform Origin = null;
    public Transform Target = null;
    public bool Connected = false;
    public Vector3 LastOriginPosition = Vector3.zero;
    public Vector3 LastTargetPosition = Vector3.zero;

    private LineRenderer _lineRenderer;
    [SerializeField] private Texture[] _textures;

    private int _animationStep = 0;
    [SerializeField] private float _fps = 12;
    [SerializeField] private float _maxLifetime = 1.0f;
    [SerializeField] private float _lightningSpeed = 50f;
    private float _lifeTime = 0f;
    private float _fpsCounter;
    private float _length = 0f;

    private void Awake ()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update ()
    {
        // Animation
        _fpsCounter += Time.deltaTime;
        if (_fpsCounter >= 1f / _fps)
        {
            _animationStep++;
            if (_animationStep >= _textures.Length)
            {
                _animationStep = 0;
            }

            _lineRenderer.material.SetTexture("_MainTex", _textures[_animationStep]);

            _fpsCounter = 0f;
        }

        // lifecycle
        if (_lifeTime > _maxLifetime)
        {
            if (BreakConnection())
            {
                Destroy(gameObject);
            }
        }
        else if (Connected)
        {
            if (Origin != null) { LastOriginPosition = Origin.position; }
            if (Target != null) { LastTargetPosition = Target.position; }
            if (Origin != null && Target != null)
            {
                SetLinePoints(LastOriginPosition, LastTargetPosition);
            }

            _lifeTime += Time.deltaTime;
        }
        else
        {
            MakeConnection();
        }
    }

    private bool MakeConnection ()
    {
        if (Origin != null)
        {
            LastOriginPosition = Origin.position;
        }
        if (Target != null)
        {
            LastTargetPosition = Target.position;
        }
        Vector3 fromTo = LastTargetPosition - LastOriginPosition;
        float distance = fromTo.magnitude;

        _length += _lightningSpeed * Time.deltaTime;
        if (_length > distance)
        {
            SetLinePoints(LastOriginPosition, LastTargetPosition);
            _length = distance;
            Connected = true;
            return true;
        }

        SetLinePoints(LastOriginPosition, LastOriginPosition + fromTo.normalized * _length);
        return false;
    }

    private bool BreakConnection ()
    {
        if (Origin != null)
        {
            LastOriginPosition = Origin.position;
        }
        if (Target != null)
        {
            LastTargetPosition = Target.position;
        }
        Vector3 fromTo = LastTargetPosition - LastOriginPosition;

        _length -= _lightningSpeed * Time.deltaTime;
        if (_length < 0f)
        {
            Connected = false;
            return true;
        }

        SetLinePoints(LastTargetPosition - fromTo.normalized * _length, LastTargetPosition);
        return false;
    }

    public void SetLinePoints(Vector3 start, Vector3 end)
    {
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}
