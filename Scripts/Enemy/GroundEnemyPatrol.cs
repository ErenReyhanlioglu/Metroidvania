using UnityEngine;

public class GroundEnemyPatrol : MonoBehaviour
{
    public Transform pointA; 
    public Transform pointB;  
    public float speed = 2f;

    private Vector3 target; 

    void Start()
    {
        target = pointA.position;  
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
            FlipSprite();  
        }
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;  
        transform.localScale = scale;
    }
}
