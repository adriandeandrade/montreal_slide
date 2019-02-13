using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEntity : MonoBehaviour, IDamageable
{
    [Header("Movement Fields")]
    [Range(1f, 150f)] [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveSpeedSmoothing = 0.5f;
    [SerializeField] protected bool facingRight;

    [Header("Other Setup")]
    [SerializeField] protected Color damageBlipColor;
    [SerializeField] protected UnityEvent OnLandEvent;

    [Header("Collision Fields")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundMask;

    [Header("Component References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rBody2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected Vector2 xMove;
    protected bool isJumping;
    protected bool isGrounded;

    private const float groundedRadius = 0.2f;
    protected Vector2 Velocity;

    protected virtual void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void FixedUpdate()
    {
        CheckForGround();
    }

    private void CheckForGround()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask); // Check for ground using ground collision.

        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != this.gameObject) // Check if we arent colliding with ourselves.
            {
                isGrounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    protected virtual void Move()
    {
        Vector2 targetVelocity = new Vector2(xMove.x * 10f, rBody2D.velocity.y);
        rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, moveSpeedSmoothing);

        if (xMove.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (xMove.x < 0 && facingRight)
        {
            Flip();
        }
    }

    protected void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = facingRight;
    }

    protected bool MouseOnLeft()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
        {
            return false;
        }
        else if (mousePos.x < transform.position.x)
        {
            return true;
        }

        return false;
    }

    public virtual void TakeDamage(int amount, Transform objectHit)
    {
        
    }
    public virtual void OnLanding()
    {

    }

}
