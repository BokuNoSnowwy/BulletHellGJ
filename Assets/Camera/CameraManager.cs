using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    [Header("Ground and camera's target")]
    [SerializeField] private Transform _target;
    [SerializeField] private SpriteRenderer _ground;

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

        UpdateCameraPosition();
    }

    #region OrthographicSize
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
    public void CalculateBorders()
    {
        if (_ground != null)
        {
            Vector3 extents = _ground.bounds.extents;

            _maxX = (_ground.transform.position.x + extents.x - _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane)).x);
            _maxY = (_ground.transform.position.y + extents.y - _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane)).y);
            _minX = (_ground.transform.position.x - extents.x - _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane)).x);
            _minY = (_ground.transform.position.y - extents.y - _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane)).y);
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 newPos = Vector2.Lerp(transform.position,_target.position, _speed * Time.deltaTime);
        newPos.z = transform.position.z;

        CheckBorders(ref newPos);

        transform.position = newPos;
    }

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
}
#endregion 