using UnityEngine;

public class FlyingEnemyPatrol : MonoBehaviour
{
    public Transform pointA; 
    public Transform pointB;  
    public float speed = 2f;
    public float waveHeight = 1f; 
    public float waveSpeed = 2f;   

    private Vector3 startPos;
    private Vector3 target;

    void Start()
    {
        startPos = transform.position;
        target = pointA.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        float waveOffset = Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position += new Vector3(0, waveOffset * Time.deltaTime, 0);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }
}
