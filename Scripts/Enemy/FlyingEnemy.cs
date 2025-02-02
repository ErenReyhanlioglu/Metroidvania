using UnityEngine;

public class FlyingEnemy : Enemy
{
    public Vector3 flightPattern;

    void Start()
    {
        flightPattern = new Vector3(1, 0, 0); 
    }

    public override void Move()
    {
        transform.position += flightPattern * speed * Time.deltaTime;
    }

    public override void Attack()
    {
        Debug.Log("FlyingEnemy saldýrýyor!");
    }
}