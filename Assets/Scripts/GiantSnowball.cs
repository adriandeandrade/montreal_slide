using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSnowball : BaseEntity
{
    [Header("Giant Snowball Setup")]
    [SerializeField] private BoxCollider2D interactCollider;
    [SerializeField] private GameObject snowBallPrefab;

    public bool doMove;

    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (!doMove) return;

        if (isGrounded)
        {
            xMove = Vector2.left * moveSpeed * Time.deltaTime;
        }
    }

    public void KillBall()
    {
        animator.SetTrigger("Break");
        //rBody2D.simulated = false;
        Invoke("Break", 0.2f);
    }

    public void Break()
    {
        int amountToSpawn = Random.Range(4, 5);

        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject snowballInstance = Instantiate(Resources.Load("Prefabs/Items/Snowball_Pickup", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D snowBallRBody = snowballInstance.GetComponent<Rigidbody2D>();

            Vector2 direction = new Vector2(Random.Range(-20f, 20f), Random.Range(-10f, 10f));
            Debug.Log(direction.normalized);
            float force = Random.Range(-20f, 20f);

            snowBallRBody.AddForce(direction.normalized * 2f, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }
}
