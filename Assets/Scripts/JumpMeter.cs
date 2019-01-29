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
        if(Input.GetKeyDown(KeyCode.Space) && !playerController.isJumping)
        {
            StartCoroutine(CalculateJumpForce());
            isCalculatingJump = true;
        }
    }

    IEnumerator CalculateJumpForce()
    {
        while (Input.GetKey(KeyCode.Space))
        {
            if(jumpForceAmount >= 1f)
            {
                isCalculatingJump = false;
                playerController.Jump(jumpForceAmount * jumpForceMultiplier);
                jumpForceAmount = 0f;
                scrollBar.value = 0f;
                yield break;
            }

            jumpForceAmount += jumpForceIncrement * Time.deltaTime;
            scrollBar.value = jumpForceAmount;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isCalculatingJump = false;
        playerController.Jump(jumpForceAmount * jumpForceMultiplier);
        jumpForceAmount = 0f;
        scrollBar.value = 0f;
        yield break;
    }
}
