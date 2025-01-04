using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTimer;
    private float dashCooldownTime;
    private Vector2 dashDirection;
    private Vector2 dashTarget;

    Rigidbody2D rb;
    CollisonCheck collisionCheck;
    CharacterEmotionSystem characterEmotionSystem;
    PlayerMovement playerMovement;
    CustomInspectorObjectsForEmotions customInsObjForEmotions;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponent<CollisonCheck>();
        characterEmotionSystem = GetComponent<CharacterEmotionSystem>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        customInsObjForEmotions = characterEmotionSystem.currentEmotion.customInsObjEmotions;

        dashCooldownTime = customInsObjForEmotions.dashCoolDown;

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (dashCooldownTimer <= 0)
        {
            if (customInsObjForEmotions.emotionType == EmotionType.Joy)
            {
                HandleJoyDash();
            }
            else if (customInsObjForEmotions.emotionType == EmotionType.Sadness)
            {
                HandleSadnessDash();
            }
            else if (customInsObjForEmotions.emotionType == EmotionType.Anger)
            {
                HandleAngerDash();
            }
        }
    }

    #region ANGER DASH
    private void HandleAngerDash()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (!collisionCheck.isDashing)
            {
                isDashing = true;
                collisionCheck.isDashing = true;
                Time.timeScale = customInsObjForEmotions.angerDashTimeScale;
                Time.fixedDeltaTime = Time.timeScale * .02f;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector2 characterPosition = transform.position;
            dashTarget = mousePosition;

            float distance = Vector3.Distance(dashTarget, characterPosition);

            if (distance > customInsObjForEmotions.angerMaxDashDistance)
            {
                Vector2 direction = (dashTarget - characterPosition).normalized;
                dashTarget = characterPosition + direction * customInsObjForEmotions.angerMaxDashDistance;
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (collisionCheck.isDashing)
            {
                Time.timeScale = 1;
                dashCooldownTimer = dashCooldownTime;
                StartCoroutine(DashToTarget());
                collisionCheck.isDashing = false;
                isDashing = false;
            }
        }
    }

    private IEnumerator DashToTarget()
    {
        yield return new WaitForSeconds(0.1f);

        Vector2 originalPosition = transform.position;
        float startTime = Time.time;

        while (Time.time < startTime + customInsObjForEmotions.dashDuration)
        {
            float t = (Time.time - startTime) / customInsObjForEmotions.dashDuration;
            rb.MovePosition(Vector2.Lerp(originalPosition, dashTarget, t));
            yield return null;
        }

        rb.MovePosition(dashTarget);

        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = Vector2.zero;

        yield return new WaitForSeconds(0.2f);

        Physics2D.gravity = originalGravity;
    }

    private void DrawAngerDashCircle()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, customInsObjForEmotions.angerMaxDashDistance);
    }

    private void DrawDashTarget()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(dashTarget, 0.2f);

        int segments = 30;
        float angle = 0f;
        float radius = Mathf.Abs(Vector3.Distance(dashTarget, transform.position));

        Vector3 prevPoint = transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

        for (int i = 1; i <= segments; i++)
        {
            angle = i * Mathf.PI * 2 / segments;
            Vector3 newPoint = transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            if (i % 2 == 0)
            {
                Gizmos.DrawLine(prevPoint, newPoint);
            }

            prevPoint = newPoint;
        }
    }
    #endregion

    #region JOY & SADNESS DASH
    private void HandleJoyDash()
    {
        HandleDash();
        // Neþe dash'i için saldýrý dodgelama
    }

    private void HandleSadnessDash()
    {
        HandleDash();
    }

    private void HandleDash()
    {
        if (customInsObjForEmotions.emotionType == EmotionType.Sadness && collisionCheck.isDashing)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SadTriggerWall"), true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SadTriggerWall"), false);
        }

        if (customInsObjForEmotions.emotionType == EmotionType.Joy && collisionCheck.isDashing)
        {
            collisionCheck.isHitable = false;
        }
        else
        {
            collisionCheck.isHitable = true;
        }

        if (collisionCheck.isDashing)
        {
            dashTime += Time.deltaTime;

            if (dashTime >= customInsObjForEmotions.dashDuration)
            {
                collisionCheck.isDashing = false;
                dashCooldownTimer = dashCooldownTime;
            }
        }
        else
        {
            if (dashCooldownTimer <= 0 && Input.GetButtonDown("Dash"))
            {
                collisionCheck.isDashing = true;
                dashTime = 0;

                dashDirection = new Vector2(playerMovement.moveInputHori, 0).normalized;

                if (playerMovement.moveInputHori == 0)
                {
                    dashDirection = playerMovement.isFacingRight ? Vector2.left : Vector2.right;
                }
            }
        }

        if (collisionCheck.isDashing)
        {
            rb.velocity = new Vector2(dashDirection.x * customInsObjForEmotions.dashSpeed, rb.velocity.y);
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (isDashing)
        {
            DrawAngerDashCircle();
            DrawDashTarget();
        }
    }
}
