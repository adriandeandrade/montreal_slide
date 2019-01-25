using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Animator animator;
    public float playerSpeed;
    public Vector2 jumpHeight;
    public bool jump = false;
    
    void Update()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        transform.position = transform.position + horizontal * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
            jump = true;
            animator.SetBool("IsJumping", true);
        }

    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        animator.SetBool("IsJumping", false); 
    }

}
