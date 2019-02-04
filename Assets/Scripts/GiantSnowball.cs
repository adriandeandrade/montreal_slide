using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSnowball : MonoBehaviour
{
    [SerializeField] private float rollSpeed;
    private Rigidbody2D rBody2D;

    private void Update()
    {
        transform.Translate(Vector2.left * rollSpeed * Time.deltaTime);
    }

    public void KillBall()
    {
        //Spawn snowballs
        Destroy(gameObject);
    }
}
