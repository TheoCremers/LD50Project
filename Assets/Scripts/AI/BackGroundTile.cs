using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundTile : MonoBehaviour
{
    private Camera _camera = null;
    private float _width = 0;
    private float _height = 0;
    private float _margin = 1f;

    void Start()
    {
        _camera = Camera.main;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _width = spriteRenderer.bounds.size.x;
        _height = spriteRenderer.bounds.size.y;
    }

    void Update()
    {
        Vector2 relativeToCamera = _camera.transform.position - transform.position;
        bool anyOffset = false;

        float horizontalOffset = 0f;
        if (relativeToCamera.x > _width + _margin)
        {
            horizontalOffset = 2f * _width;
            anyOffset = true;
        }
        else if (-relativeToCamera.x > _width + _margin)
        {
            horizontalOffset = -2f * _width;
            anyOffset = true;
        }

        float verticalOffset = 0f;
        if (relativeToCamera.y > _height + _margin)
        {
            verticalOffset = 2f * _height;
            anyOffset = true;
        }
        else if (-relativeToCamera.y > _height + _margin)
        {
            verticalOffset = -2f * _height;
            anyOffset = true;
        }

        if (anyOffset)
        {
            transform.position += new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
