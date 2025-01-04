using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SwapCams : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    [Header("Cams")]
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    [Space]

    [Header("Trigger Enable Axis")]
    public TriggerAxis triggerAxis;

    CinemachineVirtualCamera _currentCam;
    Vector2 _entryPosition;

    public enum TriggerAxis
    {
        XAxis,
        YAxis
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _entryPosition = collision.transform.position;

        if (cam1.isActiveAndEnabled && !cam2.isActiveAndEnabled)
            _currentCam = cam1;
        else if (!cam1.isActiveAndEnabled && cam2.isActiveAndEnabled)
            _currentCam = cam2;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Vector2 exitPosition = collision.transform.position;

        if (triggerAxis == TriggerAxis.XAxis)
        {
            if (_entryPosition.x < transform.position.x && exitPosition.x > transform.position.x)
            {
                SwapCamera();
            }
            else if (_entryPosition.x > transform.position.x && exitPosition.x < transform.position.x)
            {
                SwapCamera();
            }
        }
        else if (triggerAxis == TriggerAxis.YAxis)
        {
            if (_entryPosition.y < transform.position.y && exitPosition.y > transform.position.y)
            {
                SwapCamera();
            }
            else if (_entryPosition.y > transform.position.y && exitPosition.y < transform.position.y)
            {
                SwapCamera();
            }
        }
    }

    private void SwapCamera()
    {
        if (_currentCam.name == cam1.name)
        {
            cam1.enabled = false;
            cam2.enabled = true;
            _currentCam = cam2;
        }
        else if (_currentCam.name == cam2.name)
        {
            cam1.enabled = true;
            cam2.enabled = false;
            _currentCam = cam1;
        }

        if (_currentCam.name == "PlayerFollowVerticalCam" && customInspectorObjects.playerFollowVerticalCam != null && customInspectorObjects.camVertFollowObj != null)
        {
            customInspectorObjects.playerFollowVerticalCam.vertFollowGameObj = customInspectorObjects.camVertFollowObj;
        }
        else if (_currentCam.name == "PlayerLockedCam" && customInspectorObjects.playerFollowLockedCam != null && customInspectorObjects.camPinGameObject != null)
        {
            customInspectorObjects.playerFollowLockedCam.lockedPosObj = customInspectorObjects.camPinGameObject;
        }
    }
}



[System.Serializable]
public class CustomInspectorObjects
{
    public bool pinVerticalCamPos = false;
    public bool pinCam = false;

    [HideInInspector] public PlayerFollowVerticalCam playerFollowVerticalCam; 
    [HideInInspector] public PlayerLockedCam playerFollowLockedCam;
    
    [HideInInspector] public GameObject camPinGameObject;
    [HideInInspector] public GameObject camVertFollowObj;
}

[CustomEditor(typeof(SwapCams))]
public class MyScriptEditor : Editor
{
    SwapCams swapCams;

    private void OnEnable()
    {
        swapCams = (SwapCams)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (swapCams.customInspectorObjects.pinVerticalCamPos)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Vertical Camera Settings", EditorStyles.boldLabel);

            swapCams.customInspectorObjects.playerFollowVerticalCam = EditorGUILayout.ObjectField(
                "Player Follow Vertical Cam",
                swapCams.customInspectorObjects.playerFollowVerticalCam,
                typeof(PlayerFollowVerticalCam),
                true) as PlayerFollowVerticalCam;

            swapCams.customInspectorObjects.camVertFollowObj = EditorGUILayout.ObjectField(
                "Cam Vertical Follow Object",
                swapCams.customInspectorObjects.camVertFollowObj,
                typeof(GameObject),
                true) as GameObject;
        }



        if (swapCams.customInspectorObjects.pinCam)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locked Camera Settings", EditorStyles.boldLabel);

            swapCams.customInspectorObjects.playerFollowLockedCam = EditorGUILayout.ObjectField(
                "Player Locked Cam",
                swapCams.customInspectorObjects.playerFollowLockedCam,
                typeof(PlayerLockedCam),
                true) as PlayerLockedCam;

            swapCams.customInspectorObjects.camPinGameObject = EditorGUILayout.ObjectField(
                "Cam Pin Position",
                swapCams.customInspectorObjects.camPinGameObject,
                typeof(GameObject),
                true) as GameObject;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(swapCams);
        }
    }
}
