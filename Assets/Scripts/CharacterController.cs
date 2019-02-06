using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [Range(0, 0.3f)] [SerializeField] private float movementSmoothingAmount = .05f;
    [SerializeField] private float jumpHeightMultiplier;
    private float jumpAmount;
    [HideInInspector] public bool isGrounded;
    private bool jump = false;
    private const float groundedRadius = 0.2f;

    private Vector2 Velocity;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rBody2D;

    private void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = false;
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask); // Check for ground using ground collision.

        for (int i = 0; i < groundColliders.Length; i++)
        {
            if(groundColliders[i].gameObject != gameObject) // Check if we arent colliding with ourselves.
            {
                isGrounded = true;
            }
        }

        if(jump)
        {
            rBody2D.AddForce(Vector2.up * jumpAmount * jumpHeightMultiplier, ForceMode2D.Impulse);
            jump = false;
        }
    }

    public void Move(float move)
    {
        Vector2 targetVelocity = new Vector2(move * 10f, rBody2D.velocity.y);
        rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, movementSmoothingAmount);

        if(isGrounded)
        {
            isGrounded = false;
            rBody2D.AddForce(Vector2.up * jumpAmount * jumpHeightMultiplier, ForceMode2D.Impulse);
        }
    }

    public void Jump(float amount)
    {
        jumpAmount = amount;
        jump = true;
    }
}
