using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTimer;
    private Vector2 dashDirection;
    private Vector2 dashTarget;

    Rigidbody2D rb;
    CollisonCheck collisionCheck;
    CharacterEmotionSystem characterEmotionSystem;
    PlayerMovement playerMovement;
    CustomInspectorObjectsForEmotions customInsObjForEmotions;
    Animator animator;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionCheck = GetComponent<CollisonCheck>();
        characterEmotionSystem = GetComponent<CharacterEmotionSystem>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
       
        customInsObjForEmotions = characterEmotionSystem.currentEmotion.customInsObjEmotions;

       
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.performed && dashCooldownTimer <= 0 && !collisionCheck.isDashing)
        {
            animator.SetTrigger("Dash");
            switch (customInsObjForEmotions.emotionType)
            {
                case EmotionType.Joy:
                    HandleJoyDash();
                    break;
                case EmotionType.Sadness:
                    HandleSadnessDash();
                    break;
                case EmotionType.Anger:
                    HandleAngerDash();
                    break;
                default:
                    break;
            }
        }
    }

    #region ANGER DASH
    private void HandleAngerDash()
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


        StartCoroutine(DashToTarget());
    }

    private IEnumerator DashToTarget()
    {
        Vector2 originalPosition = transform.position;
        float startTime = Time.time;

        while (Time.time < startTime + customInsObjForEmotions.dashDuration)
        {
            float t = (Time.time - startTime) / customInsObjForEmotions.dashDuration;
            rb.MovePosition(Vector2.Lerp(originalPosition, dashTarget, t));
            yield return null;
        }

        rb.MovePosition(dashTarget);

        EndDash();
    }
    #endregion

    #region JOY & SADNESS DASH
    private void HandleJoyDash()
    {
        StartDirectionalDash();
    }

    private void HandleSadnessDash()
    {
        StartDirectionalDash();
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SadTriggerWall"), true);
    }

    private void StartDirectionalDash()
    {
        collisionCheck.isDashing = true;
        dashTime = 0;

        
        dashDirection = new Vector2(playerMovement.moveInputHori, 0).normalized;
        if (playerMovement.moveInputHori == 0)
        {
            dashDirection = playerMovement.isFacingRight ? Vector2.right : Vector2.left;
           
        }

        StartCoroutine(DirectionalDashCoroutine());
    }

    private IEnumerator DirectionalDashCoroutine()
    {
        while (dashTime < customInsObjForEmotions.dashDuration)
        {
            dashTime += Time.deltaTime;

            rb.velocity = new Vector2(dashDirection.x * customInsObjForEmotions.dashSpeed, rb.velocity.y);

            yield return null;
        }

        rb.velocity = Vector2.zero;
        EndDash();
    }
    #endregion

    private void EndDash()
    {
        collisionCheck.isDashing = false;
        dashCooldownTimer = customInsObjForEmotions.dashCoolDown;
        rb.velocity = Vector2.zero;
        isDashing = false;

        Time.timeScale = 1; 
        Time.fixedDeltaTime = 0.02f;
    }

    private void OnDrawGizmos()
    {
        if (isDashing)
        {
            DrawAngerDashCircle();
            DrawDashTarget();
        }
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
    }
}
