using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;
    private PlayerController playerController;

    [Header("Jump Configuration")]
    [Tooltip("This value controls the jump force multiplier.")]
    public float jumpForceMultiplier;
    
    private float jumpForceAmount;
    [Tooltip("This value controls the how fast the jump meter increments.")]
    [SerializeField] private float jumpForceIncrement;

    private bool isCalculatingJump; // Boolean for keeping track of whether or not we are calculating a jump.

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        scrollBar.value = 0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !playerController.isJumping) // Check if we press the space bar and if the player is not jumping.
        {
            StartCoroutine(CalculateJumpForce()); // We start to coroutine for calculating the jump.
            isCalculatingJump = true; // We let the class know we are calculating a jump.
        }
    }

    IEnumerator CalculateJumpForce()
    {
        while (Input.GetKey(KeyCode.Space)) // We run the code until we release the space bar.
        {
            if(jumpForceAmount >= 1f) // Since the scrollbar value only goes from 0-1, we want to make sure we dont go over 1.
            {
                isCalculatingJump = false;
                playerController.Jump(jumpForceAmount * jumpForceMultiplier);  // But if we do reach that value, we do the jump automatically.
                jumpForceAmount = 0f; // Reset the jump force value to 0.
                scrollBar.value = 0f; // Reset the scroll bar value to 0 (visual).
                yield break;
            }

            jumpForceAmount += jumpForceIncrement * Time.deltaTime; // Increment jump force amount by the multiplier we set.
            scrollBar.value = jumpForceAmount; // Set the scrollbar value to the jump force amount each deltaTime second.
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isCalculatingJump = false;
        playerController.Jump(jumpForceAmount * jumpForceMultiplier);
        jumpForceAmount = 0f;
        scrollBar.value = 0f;
        yield break;
    }
}
