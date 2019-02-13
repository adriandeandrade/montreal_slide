using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private bool knockback;
    private float knockbackCounter;
    private float knockbackTime;
    private float knockbackForce;

    private Transform otherObject;

    private Rigidbody2D rBody2D;

    private void OnEnable()
    {
        rBody2D = GetComponent<Rigidbody2D>();
    }

    public void InitKnockback(float _knockbackTime, float _knockbackForce, Transform hitDirection)
    {
        knockbackTime = _knockbackTime;
        knockbackForce = _knockbackForce;
        otherObject = hitDirection;
        knockback = true;
        ApplyKnockback();
    }


    private void Update()
    {
        HandleKnockback();
    }

    private void ApplyKnockback()
    {
        knockbackCounter = knockbackTime;
        Vector2 hitDirection = transform.position - otherObject.transform.position;
        rBody2D.velocity = hitDirection.normalized * knockbackForce;
        rBody2D.velocity = new Vector2(rBody2D.velocity.x, knockbackForce);
    }

    private void HandleKnockback()
    {
        if (knockback)
        {
            if (knockbackCounter > 0)
            {
                knockbackCounter -= Time.deltaTime;
            }
            else if (knockbackCounter <= 0)
            {
                knockback = false;
                Destroy(this);
            }
        }
    }
}
