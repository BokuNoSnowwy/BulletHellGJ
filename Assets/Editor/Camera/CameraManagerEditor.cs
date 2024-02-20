using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{
    SerializedProperty camera;
    SerializedProperty parameters;

    SerializedProperty player;
    SerializedProperty threshold;

    SerializedProperty ground;
    SerializedProperty worldSize;
    SerializedProperty worldOrigin;

    SerializedProperty useSprite;

    bool show = false;
    private void OnEnable()
    {
        camera = serializedObject.FindProperty("_camera");
        parameters = serializedObject.FindProperty("_cameraParameters");
        player = serializedObject.FindProperty("_target");
        ground = serializedObject.FindProperty("_ground");
        worldSize = serializedObject.FindProperty("WorldSize");
        worldOrigin = serializedObject.FindProperty("WorldOrigin");
        useSprite = serializedObject.FindProperty("UseSpriteForBounds");
        threshold = serializedObject.FindProperty("movementThreshold");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CameraManager manager = (CameraManager)target;

        EditorGUILayout.PropertyField(camera);
        EditorGUILayout.PropertyField(parameters);

        EditorGUILayout.PropertyField(player);
        EditorGUILayout.PropertyField(threshold);

        EditorGUILayout.PropertyField(useSprite);

        if (manager.UseSpriteForBounds)
        {
            EditorGUILayout.PropertyField(ground);
        }
        else
        {
            EditorGUILayout.PropertyField(worldSize);
            EditorGUILayout.PropertyField(worldOrigin);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
