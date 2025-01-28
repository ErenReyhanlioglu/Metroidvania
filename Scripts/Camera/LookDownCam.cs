using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDownCam : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerFollowCam playerFollowCam;

    [Header("Cameras")]
    public List<CinemachineVirtualCamera> cameras;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindActiveCam();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerFollowCam.isLookDownTriggerActive = false;
        }
    }

    private void FindActiveCam()
    {
        foreach (var cam in cameras)
        {
            if(cam.isActiveAndEnabled)
            {
                playerFollowCam.isLookDownTriggerActive = true;
                break;
            }
        }
    }
}
