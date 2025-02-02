using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public float attackPower;
    public bool playerDetected;

    public abstract void Move();  
    public abstract void Attack(); 

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
