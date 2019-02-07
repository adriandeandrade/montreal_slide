using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOLD : MonoBehaviour
{ 
    [SerializeField] private int maxSnowballs = 5;
    [SerializeField] private int currentSnowballs;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackAmount;

    public float maxSpeed;
    public float jumpHeightModifier;

    private float jumpVelocity;

    public enum FacingDirection { LEFT, RIGHT };
    private FacingDirection facing;

    public enum PlayerStates { IDLE, RUNNING, JUMPING };
    private PlayerStates playerState;

    private Vector2 movement;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PlayerShooting shooting;
    private JumpMeter jumpMeter;

    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        jumpMeter = GetComponent<JumpMeter>();
    }

    private void Start()
    {
        shooting = GetComponent<PlayerShooting>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && HasAmmo()) // Left mouse button
        {
            shooting.Shoot();
            currentSnowballs -= 1;
        }
    }

    #region Movement and Animator Code

    //protected override void ComputeVelocity()
    //{
    //    movement = Vector2.zero;
    //    movement.x = Input.GetAxis("Horizontal");

    //    UpdateFacingDirection();

    //    if (Input.GetButtonDown("Jump") && isGrounded)
    //    {
    //        StartCoroutine(jumpMeter.CalculateJumpForce());
    //    }

    //    targetVelocity = movement * maxSpeed;

    //    if (isGrounded)
    //    {
    //        animator.SetBool("IsJumping", false);
    //    }
    //    else if (!isGrounded)
    //    {
    //        animator.SetBool("IsJumping", true);
    //    }
    //}

    //public void UpdateFacingDirection()
    //{
    //    if (movement.x > 0f) // Here we check if we are moving right.
    //    {
    //        facing = FacingDirection.RIGHT; // Set our players facing direction to RIGHT.
    //        animator.SetBool("facingLeft", false); // Let the animator know that we arent facing left.
    //        isMoving = true;
    //        animator.SetBool("IsMoving", true);
    //    }
    //    else if (movement.x < 0f) // Check if we are moving left.
    //    {
    //        facing = FacingDirection.LEFT; // Set our players facing direction to LEFT.
    //        animator.SetBool("facingLeft", true); // Left the animator know that we are facing left.
    //        isMoving = true;
    //        animator.SetBool("IsMoving", true);
    //    }
    //    else if (movement.x == 0)
    //    {
    //        isMoving = false;
    //        animator.SetBool("IsMoving", false);
    //    }
    //}

    //public void SetJumpVelocity(float jumpAmount)
    //{
    //    Jump(jumpAmount * jumpHeightModifier);
    //}

    #endregion

    private bool HasAmmo()
    {
        if (currentSnowballs > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(int amount)
    {
        if (Health > 0)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GiantSnowballInteract"))
        {
            other.gameObject.GetComponentInParent<GiantSnowball>().KillBall();
            //SetJumpVelocity(0.3f);
        }
    }

}
