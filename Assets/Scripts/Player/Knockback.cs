using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackTime;
    [HideInInspector] public bool isKnockback = false;

    public float knockbackCounter;
    Vector2 direction;

    bool knockbackTimerStart = false;
    bool knockback;

    Rigidbody2D rBody;
    Player player;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (knockbackCounter > 0 && knockbackTimerStart)
        {
            rBody.gravityScale = 4f;
            knockbackCounter -= Time.deltaTime;
            Debug.Log(rBody.velocity);
        }
        else if (knockbackCounter <= 0 && knockbackTimerStart)
        {
            isKnockback = false;
            knockbackTimerStart = false;
            animator.SetBool("IsHurt", false);
            spriteRenderer.color = Color.white;
            rBody.gravityScale = 7f;
        }
    }

    private void FixedUpdate()
    {
        if (knockback)
        {
            rBody.velocity = direction.normalized * knockbackForce;
            rBody.velocity = new Vector2(rBody.velocity.x, knockbackForce);
            knockback = false;
        }
    }

    public void ApplyKnockback(Vector2 _direction, Color damageColor)
    {
        isKnockback = true;
        knockback = true;
        knockbackCounter = knockbackTime;
        knockbackTimerStart = true;
        direction = _direction;
        animator.SetBool("IsHurt", true);
        spriteRenderer.color = damageColor;
    }
}
