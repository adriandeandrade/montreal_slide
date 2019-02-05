using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;
    private Player player;

    [HideInInspector] public float jumpForceAmount;

    [Header("Jump Configuration")]
    [Tooltip("This value controls the how fast the jump meter increments.")]
    [SerializeField] private float jumpForceIncrement;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        scrollBar.value = 0f;
    }

    public IEnumerator CalculateJumpForce()
    {
        while (Input.GetButton("Jump")) // We run the code until we release the space bar.
        {
            if (jumpForceAmount >= 1f) // Since the scrollbar value only goes from 0-1, we want to make sure we dont go over 1.
            {
                player.SetJumpVelocity(jumpForceAmount);
                jumpForceAmount = 0f; // Reset the jump force value to 0.
                scrollBar.value = 0f; // Reset the scroll bar value to 0 (visual).
                yield break;
            }

            jumpForceAmount += jumpForceIncrement * Time.deltaTime; // Increment jump force amount by the multiplier we set.
            scrollBar.value = jumpForceAmount; // Set the scrollbar value to the jump force amount each deltaTime second.
            yield return new WaitForSeconds(Time.deltaTime);
        }

        player.SetJumpVelocity(jumpForceAmount);
        jumpForceAmount = 0f;
        scrollBar.value = 0f;
        yield break;
    }
}
