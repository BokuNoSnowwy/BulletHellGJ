using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraParameters", menuName = "ScriptableObject/CameraParametersSO", order = 1)]
public class CameraParameters : ScriptableObject
{
    [Range(0.1f, 10.0f)] public float CameraSpeed;

    [Range(10.0f, 50.0f)] public float Distance;
}
