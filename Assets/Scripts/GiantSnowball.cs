using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GiantSnowball : MonoBehaviour
{
    [SerializeField] private float rollSpeed;
    [SerializeField] private float movementSmoothingAmount = 0.05f;

    private Rigidbody2D rBody2D;
    private Vector2 Velocity;

    private void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = Vector2.left * 10f;
        rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, movementSmoothingAmount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable objectToDealDamage = other.gameObject.GetComponent<IDamageable>();
            if (objectToDealDamage != null)
            {
                objectToDealDamage.TakeDamage(1);
            }
        }
    }

    public void KillBall()
    {
        //Spawn snowballs
        Destroy(gameObject);
    }
}
