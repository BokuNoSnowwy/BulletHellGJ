using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera parameters")]
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraParameters _cameraParameters;
    private float Distance = 30.0f;
    private float _speed = 2.0f;

    [Space(10)]

    [Header("Camera's target")]
    [SerializeField] private Transform _target;
    [SerializeField] private float movementThreshold = 0.05f;

    [Space(10)]

    [Header("Ground and camera's target")]
    public bool UseSpriteForBounds = false;
    [SerializeField] private SpriteRenderer _ground;
    public Vector2 WorldSize = new Vector2(1f,1f);
    public Vector2 WorldOrigin = new Vector2(0f,0f);

    private float _width, _height;

    private float _minX, _minY, _maxX, _maxY;

    void Start()
    {
        _width = Screen.width; _height = Screen.height;

        if(_cameraParameters != null)
        {
            Distance = _cameraParameters.Distance;
            _speed = _cameraParameters.CameraSpeed;
        }

        SetOrthographicSize();

        CalculateBorders();
    }

    private void Update()
    {
        if(Screen.width !=  _width || Screen.height != _height)
        {
            SetOrthographicSize();
        }

        if(Vector2.Distance(transform.position, _target.position) > movementThreshold)
        {
            UpdateCameraPosition();
        }
    }

    #region OrthographicSize
    /// <summary>
    /// Sets the orthographic size of the camera depending on the device's screen resolution
    /// </summary>
    public void SetOrthographicSize()
    {
        float orthoSize = 0.0f;

        if(Screen.height < Screen.width)
        {
            orthoSize = Distance * Screen.height / Screen.width * 0.5f;
        }
        else
        {
            orthoSize = Distance * Screen.width / Screen.height * 0.5f;
        }

        _camera.orthographicSize = orthoSize;
    }
    #endregion 

    #region CameraMovements

    /// <summary>
    /// Calculates camera view limits.
    /// </summary>
    public void CalculateBorders()
    {
        //Note : _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane)) = camera top right corner 
        //       _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane)) = camera bottom left corner 

        Vector3 topRightCorner = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        Vector3 bottomLeftCorner = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));

        if (UseSpriteForBounds && _ground != null)
        {
            Vector3 extents = _ground.bounds.extents;

            _maxX = _ground.transform.position.x + extents.x - topRightCorner.x;

            _maxY = _ground.transform.position.y + extents.y - topRightCorner.y;

            _minX = _ground.transform.position.x - extents.x - bottomLeftCorner.x;

            _minY = _ground.transform.position.y - extents.y - bottomLeftCorner.y;
        }
        else
        {
            float halfWorldSizeX = WorldSize.x / 2;
            float halfWorldSizeY = WorldSize.y / 2;

            _maxX = WorldOrigin.x + halfWorldSizeX - topRightCorner.x;

            _maxY = WorldOrigin.y + halfWorldSizeY - topRightCorner.y;

            _minX = WorldOrigin.x - halfWorldSizeX - bottomLeftCorner.x;

            _minY = WorldOrigin.y - halfWorldSizeY - bottomLeftCorner.y;
        }
    }

    /// <summary>
    /// Updates the camera position
    /// </summary>
    private void UpdateCameraPosition()
    {
        Vector3 newPos = Vector2.Lerp(transform.position,_target.position, _speed * Time.deltaTime);
        newPos.z = transform.position.z;

        CheckBorders(ref newPos);

        transform.position = newPos;
    }
    
    /// <summary>
    /// Checks if the given position isn't out of world's bounds and corrects it
    /// </summary>
    /// <param name="newPos">The position's reference</param>
    private void CheckBorders(ref Vector3 newPos)
    {
        if(newPos.x < _minX)
        {
            newPos.x = transform.position.x;
        }
        if(newPos.y < _minY)
        {
            newPos.y = transform.position.y;
        }
        if(newPos.x > _maxX)
        {
            newPos.x = transform.position.x;
        }
        if(newPos.y > _maxY)
        {
            newPos.y = transform.position.y;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //debug cube to see world border size
        if (UseSpriteForBounds == false)
        {
            Gizmos.DrawWireCube(WorldOrigin, WorldSize);
        }
    }
}
#endregion 

