using Cinemachine;
using UnityEngine;

public class PlayerFollowVerticalCam : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform camFollowObj;

    [Header("Scripts")]
    public PlayerMovement playerMovement;

    [Header("Camera Follow Parameters")]
    [Range(0f, 15f)]
    public float horizontalSmoothSpeed = 5f;

    [HideInInspector] public GameObject vertFollowGameObj;

    private CinemachineVirtualCamera cam;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        UpdateCamFollowObjectPosition();
    }

    private void UpdateCamFollowObjectPosition()
    {
        if (vertFollowGameObj == null || camFollowObj == null)
            return;

        Vector3 targetPosition = camFollowObj.position;
        targetPosition.y = vertFollowGameObj.transform.position.y;

        if (playerMovement.isFacingRight)
            targetPosition.x = playerMovement.transform.position.x - 1f;
        else
            targetPosition.x = playerMovement.transform.position.x + 1f;

        camFollowObj.position = Vector3.Lerp(camFollowObj.position, targetPosition, horizontalSmoothSpeed * Time.deltaTime);
    }
}
