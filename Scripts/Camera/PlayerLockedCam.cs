using Cinemachine;
using UnityEngine;

public class PlayerLockedCam : MonoBehaviour
{
    [Header("Camera Follow Parameters")]
    [Range(0f, 15f)]
    public float smoothSpeed = 5f;

    [HideInInspector] public GameObject lockedPosObj;

    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer framingTransposer;

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        if (lockedPosObj != null)
        {
            LockeCamPos();
        }
    }

    public void LockeCamPos()
    {
        framingTransposer.m_TrackedObjectOffset = Vector3.zero;
        framingTransposer.m_DeadZoneWidth = 0;
        framingTransposer.m_DeadZoneHeight = 0;
        cam.Follow = lockedPosObj.transform;
    }
}
