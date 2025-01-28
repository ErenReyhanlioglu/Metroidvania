
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonCheck : MonoBehaviour
{
    [Header("Collison Parameters")] // Gereksiz raycastler, kaldýrýlacak. Raycast yerine overlap circle kullanýlabilir.
    public Transform[] groundCheck;
    public Transform[] ceilCheck;
    public Transform leftSideCheck;
    public Transform rightSideCheck;
    [Space]
    public float collisonCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public bool isLeftSideHoldable;
    public bool isRightSideHoldable;
    public bool isRightLeftWalkable;
    public bool isRightSideWalkable;
    public bool isCeiled;
    public bool isGrounded;
    public bool isFalling;
    public bool isRising;
    public bool isWalking; // isWalking dash attýktan sonra yaklasýk 50 ms geç güncelleniyor
    public bool isDashing;
    public bool isHoldinWall;
    public bool isHitable;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        if (rb.velocity.y < 0 && !isGrounded)
        {
            isFalling = true;
            isRising = false;
        }
        else if (rb.velocity.y > 0 && !isGrounded)
        {
            isFalling = false;
            isRising = true;
        }
        else if (rb.velocity.y == 0 && (isGrounded || isCeiled)) 
        {
            isFalling = false;
            isRising = false;
        }

        if (rb.velocity.x != 0 && isGrounded && !isDashing)
            isWalking = true;
        else
            isWalking = false;
    }

    internal void ControlCollisions()
    {
        isCeiled = false;
        isGrounded = false;

        isLeftSideHoldable = Physics2D.OverlapCircle(leftSideCheck.position, collisonCheckRadius, groundLayer);
        isRightSideHoldable = Physics2D.OverlapCircle(rightSideCheck.position, collisonCheckRadius, groundLayer);

        foreach (Transform t in ceilCheck)
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, Vector2.up, collisonCheckRadius, groundLayer);

            if (hit.collider != null)
            {
                isCeiled = true;
                break;
            }
        }

        foreach (Transform t in groundCheck)
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, Vector2.down, collisonCheckRadius, groundLayer);

            if (hit.collider != null)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnDrawGizmos()
    {
        #region OVERLAPCÝRCLE
        foreach (Transform t in groundCheck)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(t.position, t.position + Vector3.down * collisonCheckRadius);
        }

        foreach (Transform t in ceilCheck)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(t.position, t.position + Vector3.up * collisonCheckRadius);
        }

        foreach (Transform t in leftSideCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(t.position, collisonCheckRadius);
        }

        foreach (Transform t in rightSideCheck)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(t.position, collisonCheckRadius);
        }
        #endregion
    }
}
