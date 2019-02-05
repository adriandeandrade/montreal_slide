using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSnowball : PhysicsObject
{
    [SerializeField] private float rollSpeed;
    private Rigidbody2D rBody2D;

    protected override void Update()
    {
        base.Update();
    }

    protected override void ComputeVelocity()
    {
        targetVelocity = Vector2.left * rollSpeed;
    }

    public void KillBall()
    {
        //Spawn snowballs
        Destroy(gameObject);
    }
}
