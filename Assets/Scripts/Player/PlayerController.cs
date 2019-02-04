using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Player player;

    private bool isMoving;
    public bool isJumping;

    [Header("Player Parameters")]
    [SerializeField] private float moveSpeed;

    private Vector3 movement; // Global variable so we can access the players movement in another function (Ex. We reference it in the UpdateFacingDirection function.

    public enum FacingDirection { LEFT, RIGHT }; // This enum is used to keep track of the players facing direction.
    private FacingDirection facing;

    public enum PlayerStates { IDLE, RUNNING, JUMPING }; // This enum is used to keep track of the players current state.
    private PlayerStates playerState;

    private Rigidbody2D rBody2D;

    private void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GetComponent<Player>();
        facing = FacingDirection.RIGHT; // Here we just initialize some variables.
        playerState = PlayerStates.IDLE;
        isMoving = false;
        isJumping = false;
    }

    private void Update()
    {
        Movement(); // Do movement code every frame.
    }

    public void Movement()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f); // Get the horizontal input and set it to the global movement variable.
        transform.Translate(movement * moveSpeed * Time.deltaTime); // Move the player using the input we just got.
        UpdateFacingDirection();

        if (movement.x > 0 || movement.x < 0) // Check if we are moving left or right.
        {
            isMoving = true;
            playerState = PlayerStates.RUNNING; // Set our players state to RUNNING.
            animator.SetFloat("Horizontal", movement.x);
            animator.SetBool("IsMoving", true); // Let the animator know we are moving by setting its IsMoving parameter to true.
        }
        else if (movement.x == 0) // Check if we arent moving.
        {
            isMoving = false;
            //rBody2D.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false); // Let the animator know we have stopped moving by setting IsMoving to false.
            playerState = PlayerStates.IDLE; // Set the players state to idle since we arent moving.
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            animator.SetBool("IsJumping", isJumping);
        }
    }

    public void Jump(float jumpForce)
    {
        rBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void UpdateFacingDirection()
    {
        if (movement.x > 0f) // Here we check if we are moving right.
        {
            facing = FacingDirection.RIGHT; // Set our players facing direction to RIGHT.
            animator.SetBool("facingLeft", false); // Let the animator know that we arent facing left.
        }
        else if (movement.x < 0f) // Check if we are moving left.
        {
            facing = FacingDirection.LEFT; // Set our players facing direction to LEFT.
            animator.SetBool("facingLeft", true); // Left the animator know that we are facing left.

        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        animator.SetBool("IsJumping", false);
        isJumping = false;

        if(other.gameObject.CompareTag("GiantSnowball"))
        {
            Debug.Log("Took damage");
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GiantSnowball"))
        {
            other.gameObject.GetComponent<GiantSnowball>().KillBall();
            Jump(3f);
        }
    }
}
