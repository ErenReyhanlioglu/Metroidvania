using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothGravity : MonoBehaviour
{
    [Header("Gravity Parameters")]
    [Range(0f, 15f)]
    public float fallMultiplier = 2.5f;
    [Range(0f, 12f)]
    public float lowJumpMultiplier = 2f;

    public LayerMask groundLayer;

    private Rigidbody2D rb;
    CollisonCheck collisionCheck;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponent<CollisonCheck>();
    }

    void FixedUpdate()
    {
        ApplySmoothGravity();
    }

    private void ApplySmoothGravity()
    {
        if (!collisionCheck.isGrounded && !collisionCheck.isHoldinWall)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}
