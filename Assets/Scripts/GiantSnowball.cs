using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GiantSnowball : MonoBehaviour
{
    [Header("Movement Fields")]
    [SerializeField] private float rollSpeed;
    [SerializeField] private float movementSmoothingAmount = .05f;
    [SerializeField] private bool movingLeft = true;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackTime;
    private float knockBackCounter;

    private bool isGrounded;
    [SerializeField] private Animator animator;

    [Header("Collision Fields")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask groundMask;
    private RaycastHit2D hit;
    [SerializeField] private float collisionCastDistance = 2f;
    [SerializeField] private float groundCollisionCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private BoxCollider2D interactCollider;
    [SerializeField] private CircleCollider2D damageCollider;

    private Rigidbody2D rBody2D;
    private Vector2 Velocity;

    private void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (knockBackCounter <= 0)
        {
            Move();
        }
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        isGrounded = false;
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCollisionCheckRadius, groundMask); // Check for ground using ground collision.

        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != this.gameObject) // Check if we arent colliding with ourselves.
            {
                isGrounded = true;
            }
        }

        //// Check for player
        //if (movingLeft)
        //{
        //    Vector2 dir = transform.TransformDirection(Vector2.left);
        //    hit = Physics2D.Raycast(transform.position, dir, collisionCastDistance, playerMask);

        //    if (hit)
        //    {
        //        Vector2 hitDirection = transform.position - hit.transform.position;
        //        hitDirection = hitDirection.normalized;
        //        hit.transform.gameObject.GetComponent<Player>().Knockback(hitDirection);
        //        Knockback(-hitDirection);
        //    }

        //    Debug.DrawRay(transform.position, dir * collisionCastDistance, Color.red);
        //}
    }

    private void Move()
    {
        if (movingLeft && isGrounded)
        {
            Vector2 targetVelocity = Vector2.left * rollSpeed;
            rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, movementSmoothingAmount);
            animator.SetBool("IsRolling", true);
        }
        else if (!movingLeft && isGrounded)
        {
            Vector2 targetVelocity = Vector2.right * rollSpeed;
            rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, movementSmoothingAmount);
        }
    }

    public void Knockback(Vector2 direction)
    {
        knockBackCounter = knockBackTime;
        rBody2D.velocity = -direction * knockBackForce;
        rBody2D.velocity = new Vector2(rBody2D.velocity.x, knockBackForce);
    }

    public void KillBall()
    {
        animator.SetTrigger("Break");
        rBody2D.simulated = false;
        //damageCollider.enabled = false;
        //interactCollider.enabled = false;
        Invoke("Break", 1.5f);
    }

    public void Break()
    {
        Destroy(gameObject);
    }
}
