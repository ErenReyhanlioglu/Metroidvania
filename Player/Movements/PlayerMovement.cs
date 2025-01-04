using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    internal float moveInputHori;
    internal Vector2 velocityHori;

    float targetVelocityX;
    float velocityXSmoothing;

    [Header("Ground Movement Parameters")]
    [Range(0f, 2f)]
    public float accelerationTime = 0.1f;
    [Range(0f, 2f)]
    public float decelerationTime = 0.1f;
    [Range(0f, 2f)]
    public float groundFriction = 0.1f;

    [Header("Air Control Parameters")]
    [Range(0f, 2f)]
    public float airAccelerationTime = 0.05f;
    [Range(0f, 2f)]
    public float airDecelerationTime = 0.2f;
    [Range(0f, 2f)]
    public float airFriction = 0.05f;

    [Header("Jump Asist Parameters")]
    [Range(0f, 4f)]
    public float coyoteTime = 0.2f;
    [Range(0f, 4f)]
    public float jumpBufferTime = 0.2f;
    [Range(0f, 4f)]
    public float jumpCooldown = 0.2f;

    internal float jumpBufferCounter;
    float coyoteTimeCounter;
    float jumpCooldownCounter;

    [Header("Wall Jump Parameters")]
    [Range(0f, 2f)]
    public float wallJumpCoyoteTime = 0.2f;
    [Range(0f, 5f)]
    public float wallClimbSpeed = 0.2f;

    float wallJumpCoyoteCounter = 0f;

    internal bool isFacingRight;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    CollisonCheck collisionCheck;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponent<CollisonCheck>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        collisionCheck.ControlCollisions();
        JumpAsists();
        HoldWall();
        FlipCharacter();
    }

    private void HandleMovement()
    {
        moveInputHori = Input.GetAxis("Horizontal");

        if (collisionCheck.isHoldinWall || collisionCheck.isDashing)
            return;

        targetVelocityX = moveInputHori * moveSpeed;

        float friction = collisionCheck.isGrounded ? groundFriction : airFriction;
        float currentAccelerationTime = collisionCheck.isGrounded ? accelerationTime : airAccelerationTime;
        float currentDecelerationTime = collisionCheck.isGrounded ? decelerationTime : airDecelerationTime;

        velocityHori.x = Mathf.SmoothDamp(rb.velocity.x, targetVelocityX, ref velocityXSmoothing,
                                      moveInputHori != 0 ? currentAccelerationTime : currentDecelerationTime + friction);
        rb.velocity = new Vector2(velocityHori.x, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCooldownCounter < 0)
        {
            if (collisionCheck.isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCooldownCounter = jumpCooldown; 
            }
            else
            {
                if (coyoteTimeCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    coyoteTimeCounter = 0;
                    jumpCooldownCounter = jumpCooldown;
                }

                if (wallJumpCoyoteCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    wallJumpCoyoteCounter = 0;
                    jumpCooldownCounter = jumpCooldown;
                }
            }
        }
    }

    private void JumpAsists()
    {
        if (Input.GetButtonDown("Jump") && !collisionCheck.isGrounded)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        if (collisionCheck.isGrounded && jumpBufferCounter > 0 && jumpCooldownCounter < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCooldownCounter = jumpCooldown;
            jumpBufferCounter = 0;
        }

        jumpCooldownCounter -= Time.deltaTime;

        if (collisionCheck.isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (collisionCheck.isHoldinWall)
            wallJumpCoyoteCounter = wallJumpCoyoteTime;
        else
            wallJumpCoyoteCounter -= Time.deltaTime;
    }

    private void FlipCharacter()
    {
        if ((velocityHori.x > 0 && isFacingRight) || (velocityHori.x < 0 && !isFacingRight))
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    private void HoldWall()
    {
        if (Input.GetButton("HoldWall"))
        {
            if (moveInputHori > 0 && collisionCheck.isRightSideHoldable)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                collisionCheck.isHoldinWall = true;
                rb.gravityScale = 0;
                wallJumpCoyoteCounter = wallJumpCoyoteTime;
            }
            else if (moveInputHori < 0 && collisionCheck.isLeftSideHoldable)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                collisionCheck.isHoldinWall = true;
                rb.gravityScale = 0;
                wallJumpCoyoteCounter = wallJumpCoyoteTime;
            }
        }
        else
        {
            collisionCheck.isHoldinWall = false;
            rb.gravityScale = 1;
        }
    }
}
