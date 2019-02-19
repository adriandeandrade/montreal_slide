using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : BaseEntity
{
    [Header("Player Setup")]
    [SerializeField] private float jumpHeightModifier;
    [SerializeField] private float minJumpHeight;

    [Header("Player Components")]
    [SerializeField] private JumpMeter jumpMeter;
    [SerializeField] private GameObject jumpMeterUI;

    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool facingRight;

    float jumpAmount;

    Player player;
    Animator animator;
    Knockback knockback;

    protected override void Awake()
    {
        base.Awake();

        jumpMeter = GetComponent<JumpMeter>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        jumpMeterUI.SetActive(false);
    }

    protected override void Update()
    {
        if (knockback.knockbackCounter <= 0)
        {
            xMove = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f);
        }

        animator.SetFloat("Speed", Mathf.Abs(xMove.x));

        if (xMove.x < 0 && facingRight) player.Flip();
        else if (xMove.x > 0 && !facingRight) player.Flip();

        if (Input.GetKeyDown(KeyCode.Space) && !knockback.isKnockback && !isJumping)
        {
            jumpMeterUI.SetActive(true);
            StartCoroutine(jumpMeter.CalculateJumpForce());
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (knockback.knockbackCounter <= 0)
        {
            Move();
        }

        if (isJumping)
        {
            if (jumpAmount <= 0.15f)
            {
                rBody2D.AddForce(Vector2.up * minJumpHeight, ForceMode2D.Impulse);
            } else
            {
                Vector2 jumpForce = new Vector2(rBody2D.velocity.x, Mathf.RoundToInt(minJumpHeight + (jumpAmount * 10) * jumpHeightModifier));
                rBody2D.AddForce(jumpForce, ForceMode2D.Impulse);
            }
            
            isJumping = false;
        }
    }

    public void CalculateJump(float amount)
    {
        jumpMeterUI.SetActive(false);
        jumpAmount = amount * 0.5f;
        isJumping = true;
        animator.SetBool("IsJumping", true);
        AudioManager.instance.Play("player_jump_moan");
    }

    public override void OnLanding()
    {
        base.OnLanding();
        isJumping = false;
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsHurt", false);
        AudioManager.instance.Play("player_landing");
    }
}
