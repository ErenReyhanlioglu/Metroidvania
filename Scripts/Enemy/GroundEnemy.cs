using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    public bool isJumping;

    void Start()
    {
        isJumping = false;
    }

    public override void Move()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public override void Attack()
    {
        Debug.Log("GroundEnemy saldýrýyor!");
    }
}

