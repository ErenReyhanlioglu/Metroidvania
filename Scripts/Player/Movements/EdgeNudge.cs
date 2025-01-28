using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeNudge : MonoBehaviour
{
    [Header("Edge Nudging Parameters")]
    [Range(0f, 1f)]
    public float edgeNudgeHoriDist = 0.1f;
    [Range(0f, 1f)]
    public float edgeNudgeVertDist = 0.1f;
    [Range(0f, 0.1f)]
    public float edgeNudgeCloseRadius = 0.09f;
    [Range(0f, 0.1f)]
    public float edgeNudgeFarRadius = 0.05f;
    [Space]
    public Transform[] underRayCastPoints;
    public Transform[] upperRayCastPoints;
    public Transform[] leftSideRayCastPoints;
    public Transform[] rightSideRayCastPoints;

    float edgeNudgeBufferTime = 0.1f;
    internal float edgeNudgeTimer = 0;
    bool[] isNearUnderEdge;
    bool[] isNearUpperEdge;
    bool[] isNearLeftEdge;
    bool[] isNearRightEdge;
    bool[] edgeNudgeBools;

    Rigidbody2D rb;
    PlayerMovement playerMovement;
    CollisonCheck collisonCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        collisonCheck = GetComponent<CollisonCheck>();

        isNearUnderEdge = new bool[2];
        isNearUpperEdge = new bool[2];
        isNearLeftEdge = new bool[2];
        isNearRightEdge = new bool[2];
        edgeNudgeBools = new bool[4];
    }

    void Update()
    {
        if (rb.velocity.y < 0)
            edgeNudgeTimer = edgeNudgeBufferTime;

        if (!collisonCheck.isFalling)
        {
            edgeNudgeTimer -= Time.deltaTime;
            if (edgeNudgeTimer < 0) edgeNudgeTimer = 0;
        }

        CheckEdgeNudge();
        ApplyEdgeNudge();
    }

    void CheckEdgeNudge()
    {
        Array.Fill(isNearUnderEdge, false);
        Array.Fill(isNearUpperEdge, false);
        Array.Fill(isNearLeftEdge, false);
        Array.Fill(isNearRightEdge, false);

        CheckRaycasts(underRayCastPoints, Vector2.down, edgeNudgeBools, edgeNudgeCloseRadius, edgeNudgeFarRadius);
        if (edgeNudgeBools[0] && !edgeNudgeBools[1])
            isNearUnderEdge[0] = true;
        if (!edgeNudgeBools[2] && edgeNudgeBools[3])
            isNearUnderEdge[1] = true;

        CheckRaycasts(upperRayCastPoints, Vector2.up, edgeNudgeBools, edgeNudgeCloseRadius, edgeNudgeFarRadius);
        if (edgeNudgeBools[0] && !edgeNudgeBools[1])
            isNearUpperEdge[0] = true;
        if (!edgeNudgeBools[2] && edgeNudgeBools[3])
            isNearUpperEdge[1] = true;

        CheckRaycasts(leftSideRayCastPoints, Vector2.left, edgeNudgeBools, edgeNudgeCloseRadius, edgeNudgeFarRadius);
        if (edgeNudgeBools[0] && !edgeNudgeBools[1])
            isNearLeftEdge[0] = true;
        if (!edgeNudgeBools[2] && edgeNudgeBools[3])
            isNearLeftEdge[1] = true;

        CheckRaycasts(rightSideRayCastPoints, Vector2.right, edgeNudgeBools, edgeNudgeCloseRadius, edgeNudgeFarRadius);
        if (edgeNudgeBools[0] && !edgeNudgeBools[1])
            isNearRightEdge[0] = true;
        if (!edgeNudgeBools[2] && edgeNudgeBools[3])
            isNearRightEdge[1] = true;
    }


    void CheckRaycasts(Transform[] rayCastPoints, Vector2 direction, bool[] edgeArray, float closeRadius, float farRadius)
    {
        for (int i = 0; i < rayCastPoints.Length; i++)
        {
            float rayDistance = (i == 0 || i == rayCastPoints.Length - 1) ? farRadius : closeRadius;
            Vector2 startPosition = rayCastPoints[i].position;

            RaycastHit2D edgeNudgeHit = Physics2D.Raycast(startPosition, direction, rayDistance, collisonCheck.groundLayer);

            if (i < edgeArray.Length)
            {
                edgeArray[i] = edgeNudgeHit.collider != null;
            }
        }
    }


    void ApplyEdgeNudge()
    {
        Vector2 targetPosition = transform.position;

        if (edgeNudgeTimer > 0)
        {
            if (isNearUnderEdge[0] && playerMovement.moveInputHori >= 0)
                targetPosition += new Vector2(edgeNudgeHoriDist, -edgeNudgeVertDist);
            if (isNearUnderEdge[1] && playerMovement.moveInputHori <= 0)
                targetPosition += new Vector2(-edgeNudgeHoriDist, -edgeNudgeVertDist);

            if (isNearUpperEdge[0])
                targetPosition += new Vector2(edgeNudgeHoriDist, edgeNudgeVertDist);
            if (isNearUpperEdge[1])
                targetPosition += new Vector2(-edgeNudgeHoriDist, edgeNudgeVertDist);

            if (isNearLeftEdge[0] && playerMovement.moveInputHori < 0)
                targetPosition += new Vector2(-edgeNudgeHoriDist, -edgeNudgeVertDist);
            if (isNearLeftEdge[1] && playerMovement.moveInputHori < 0)
                targetPosition += new Vector2(-edgeNudgeHoriDist, edgeNudgeVertDist);

            if (isNearRightEdge[0] && playerMovement.moveInputHori > 0)
                targetPosition += new Vector2(edgeNudgeHoriDist, -edgeNudgeVertDist);
            if (isNearRightEdge[1] && playerMovement.moveInputHori > 0)
                targetPosition += new Vector2(edgeNudgeHoriDist, edgeNudgeVertDist);
        }

        Vector2 currentVelocity = Vector2.zero;
        if (targetPosition.x != transform.position.x && targetPosition.y != transform.position.y)
            rb.MovePosition(Vector2.SmoothDamp(rb.position, targetPosition, ref currentVelocity, 0.1f));
    }

    private void OnDrawGizmos()
    {
        DrawRaycastGizmos(underRayCastPoints, Vector2.down);
        DrawRaycastGizmos(upperRayCastPoints, Vector2.up);
        DrawRaycastGizmos(leftSideRayCastPoints, Vector2.left);
        DrawRaycastGizmos(rightSideRayCastPoints, Vector2.right);
    }

    private void DrawRaycastGizmos(Transform[] rayCastPoints, Vector2 direction)
    {
        float rayDistance;

        foreach (Transform t in rayCastPoints)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(t.position, 0.01f);
        }

        for (int i = 0; i < rayCastPoints.Length; i++)
        {
            rayDistance = (i == 0 || i == rayCastPoints.Length - 1) ? edgeNudgeFarRadius : edgeNudgeCloseRadius;
            Gizmos.color = Color.gray;
            Gizmos.DrawRay(rayCastPoints[i].position, direction * rayDistance);
        }
    }
}
