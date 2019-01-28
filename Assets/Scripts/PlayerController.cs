using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed;
    public bool isMoving;
    public bool isJumping;
    public Vector2 jumpHeight;
    public Vector3 movement;

    public enum FacingDirection { LEFT, RIGHT };
    public FacingDirection facing;

    public enum PlayerStates { IDLE, RUNNING, JUMPING };
    public PlayerStates playerState;

    private Rigidbody2D rBody2D;

    private void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        facing = FacingDirection.RIGHT;
        playerState = PlayerStates.IDLE;
        isMoving = false;
        isJumping = false;
    }

    private void Update()
    {
        Movement();
    }

    public void Movement()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
        UpdateFacingDirection();

        if (movement.x > 0 || movement.x < 0)
        {
            isMoving = true;
            playerState = PlayerStates.RUNNING;
            //animator.SetFloat("Horizontal", movement.x);
            animator.SetBool("IsMoving", true);
        }
        else if (movement.x == 0)
        {
            isMoving = false;
            animator.SetBool("IsMoving", false);
            playerState = PlayerStates.IDLE;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
            isJumping = true;
            animator.SetBool("IsJumping", isJumping);
        }
    }

    public void UpdateFacingDirection()
    {
        if (movement.x > 0f)
        {
            facing = FacingDirection.RIGHT;
            animator.SetBool("facingLeft", false);
        }
        else if (movement.x < 0f)
        {
            facing = FacingDirection.LEFT;
            animator.SetBool("facingLeft", true);

        }
    }
}
