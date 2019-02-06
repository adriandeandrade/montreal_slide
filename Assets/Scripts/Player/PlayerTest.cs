using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private JumpMeter jumpMeter;

    private float xMove = 0f;
    [Range(1f, 150f)] [SerializeField] private float moveSpeed;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if(Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            StartCoroutine(jumpMeter.CalculateJumpForce());
        }
    }

    private void FixedUpdate()
    {
        characterController.Move(xMove * Time.fixedDeltaTime);
    }
}
