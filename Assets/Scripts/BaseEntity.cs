using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEntity : MonoBehaviour
{
    [Header("Movement Fields")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveSpeedSmoothing = 0.5f;

    [Header("Events")]
    [SerializeField] protected UnityEvent OnLandEvent;

    [Header("Collision Fields")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundMask;
    
    protected Rigidbody2D rBody2D;

    protected Vector2 xMove;
    [HideInInspector]public bool isGrounded;

    private const float groundedRadius = 0.2f;
    protected Vector2 Velocity;

    protected virtual void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();

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
    }
    
    public virtual void OnLanding()
    {

    }

}
