using UnityEngine;
using Cinemachine;

public class PlayerFollowCam : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerMovement playerMovement;
    public CollisonCheck collisonCheck;

    [Header("Camera Follow Parameters")]
    [Range(0f, 15f)]
    public float horizontalSmoothSpeed = 5f;
    [Range(0f, 15f)]
    public float verticalSmoothSpeedUp = 2f;
    [Range(0f, 15f)]
    public float verticalSmoothSpeedDown = 10f;

    internal bool isLookDownTriggerActive;

    CinemachineVirtualCamera cam;
    CinemachineFramingTransposer framingTransposer;
    Vector3 targetOffset;

    private void Start()
    {
        isLookDownTriggerActive = false;
        cam = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        targetOffset = framingTransposer.m_TrackedObjectOffset;
    }

    void Update()
    {
        FlipCameraFocusPoint();
        SmoothTransition();
        VerticalTransition();
    }

    private void FlipCameraFocusPoint()
    {
        if (playerMovement.isFacingRight)
        {
            targetOffset.x = -1;
        }
        else
        {
            targetOffset.x = 1;
        }
    }

    private void VerticalTransition()
    {
        if (collisonCheck.isRising && !collisonCheck.isGrounded)
        {
            targetOffset.y = 1;
        }
        else if (collisonCheck.isFalling && !collisonCheck.isGrounded) 
        {
            targetOffset.y = -2;
        }
        else if(collisonCheck.isGrounded && !isLookDownTriggerActive)
        {
            targetOffset.y = 0;
        }
        else if(collisonCheck.isGrounded && isLookDownTriggerActive) // LookDown Trigger
        {
            targetOffset.y = -2.5f;
        }
    }

    private void SmoothTransition()
    {
        float currentSmoothSpeed = horizontalSmoothSpeed;

        if (collisonCheck.isRising)
        {
            currentSmoothSpeed = verticalSmoothSpeedUp;
        }
        else if (collisonCheck.isFalling)
        {
            currentSmoothSpeed = verticalSmoothSpeedDown;
        }

        framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
            framingTransposer.m_TrackedObjectOffset,
            targetOffset,
            currentSmoothSpeed * Time.deltaTime);
    }
}
