using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private JumpMeter jumpMeter;
    private Rigidbody2D rBody2D;

    [Header("Player Fields")]
    [Range(1f, 150f)] [SerializeField] private float moveSpeed;
    [SerializeField] private float movementSmoothingAmount = .05f;
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackTime;
    private float knockBackCounter;

    private float xMove = 0f;
    private float jumpAmount;
    private bool isGrounded;
    private bool jump;
    private Vector2 Velocity;

    [Header("Collision Fields")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private const float groundedRadius = 0.2f;

    private void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        jumpMeter = GetComponent<JumpMeter>();
    }

    private void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && knockBackCounter <= 0)
        {
            StartCoroutine(jumpMeter.CalculateJumpForce());
        }

        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (knockBackCounter <= 0)
        {
            Move(xMove);

            if (jump)
            {
                rBody2D.AddForce(Vector2.up * jumpAmount * jumpHeightMultiplier, ForceMode2D.Impulse);
                jump = false;
            }
        }

        CheckCollisions();
    }

    private void CheckCollisions()
    {
        isGrounded = false;
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask); // Check for ground using ground collision.

        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != this.gameObject) // Check if we arent colliding with ourselves.
            {
                isGrounded = true;
            }
        }
    }

    public void Move(float move)
    {
        Vector2 targetVelocity = new Vector2(move * 10f, rBody2D.velocity.y);
        rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, movementSmoothingAmount);
    }

    public void Jump(float amount)
    {
        jumpAmount = amount;
        jump = true;
    }

    public void Knockback(Vector2 direction)
    {
        knockBackCounter = knockBackTime;
        rBody2D.velocity = -direction * knockBackForce;
        rBody2D.velocity = new Vector2(rBody2D.velocity.x, knockBackForce);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GiantSnowballInteract"))
        {
            Jump(0.8f);
            Destroy(other.GetComponentInParent<GiantSnowball>().gameObject);
        }
        else if (other.CompareTag("GiantSnowball"))
        {
            Vector2 hitDirection = transform.position - other.transform.position;
            hitDirection = hitDirection.normalized;
            other.GetComponentInParent<GiantSnowball>().Knockback(hitDirection);
            Knockback(-hitDirection);
        }


    }
}
