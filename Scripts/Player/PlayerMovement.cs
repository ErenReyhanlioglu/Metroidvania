using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public float moveInputHori;
    public Vector2 velocityHori;

    float targetVelocityX;
    float velocityXSmoothing;
    [SerializeField] private InputAction holdWallAction;

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
    Animator animator;
    private void Start()
    {  
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponent<CollisonCheck>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       
        HandleMovement();
        collisionCheck.ControlCollisions();
        FlipCharacter();
        JumpAsists();
    }
    #region PlayerInput

   
    public void Move(InputAction.CallbackContext context)
    {
        moveInputHori = context.ReadValue<Vector2>().x;
     
    }

 

    #endregion
    private void HandleMovement()
    {
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

    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            if (collisionCheck.isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCooldownCounter = jumpCooldown;

                if (animator != null)
                {
                    animator.SetTrigger("jump");
                }
            }
            else
            {
                if (coyoteTimeCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    coyoteTimeCounter = 0;
                    jumpCooldownCounter = jumpCooldown;

                    if (animator != null)
                    {
                        animator.SetTrigger("jump");
                    }
                }

                if (wallJumpCoyoteCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    wallJumpCoyoteCounter = 0;
                    jumpCooldownCounter = jumpCooldown;

                    if (animator != null)
                    {
                        animator.SetTrigger("jump");
                    }
                }
            }
        }
    }

    private void JumpAsists()
    {
        if (!collisionCheck.isGrounded)
            jumpBufferCounter = jumpBufferTime;
        else
        jumpBufferCounter -= Time.deltaTime;
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

    public void HoldWall(InputAction.CallbackContext context)
    {
        if (context.performed) 
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
