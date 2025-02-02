using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask enemyLayer;
    private float checkInterval = 0.2f;
    private float nextCheckTime = 0f;
    private HashSet<Enemy> detectedEnemies = new HashSet<Enemy>(); 

    private void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            DetectEnemies();
            nextCheckTime = Time.time + checkInterval;
        }
    }

    void DetectEnemies()
    {
        Collider2D[] currentDetected = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        HashSet<Enemy> newDetectedEnemies = new HashSet<Enemy>(); 

        foreach (Collider2D enemyCollider in currentDetected)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (!enemy.playerDetected) 
                {
                    enemy.playerDetected = true;
                }

                newDetectedEnemies.Add(enemy);
            }
        }

        foreach (Enemy enemy in detectedEnemies)
        {
            if (!newDetectedEnemies.Contains(enemy)) 
            {
                if (enemy.playerDetected) 
                {
                    enemy.playerDetected = false;
                    Debug.Log("Düþman görüþ alanýndan çýktý: " + enemy.gameObject.name);
                }
            }
        }

        detectedEnemies = newDetectedEnemies;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
