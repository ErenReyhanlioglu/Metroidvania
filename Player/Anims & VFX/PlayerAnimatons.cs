using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimatons : MonoBehaviour
{
    private CollisonCheck collisonCheck;
    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collisonCheck = GetComponent<CollisonCheck>();
    }

    void Update()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isFalling", collisonCheck.isFalling);
        anim.SetBool("isRising", collisonCheck.isRising);
        anim.SetBool("isFalling", collisonCheck.isFalling);
        anim.SetBool("isHoldingWall", collisonCheck.isHoldinWall);
        anim.SetBool("isDashing", collisonCheck.isDashing);
        anim.SetBool("isJumping", !collisonCheck.isGrounded);
    }
}
