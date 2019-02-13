using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSnowball : BaseEntity
{
    [Header("Giant Snowball Setup")]
    [SerializeField] private BoxCollider2D interactCollider;
    [SerializeField] private GameObject snowBallPrefab;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        xMove = Vector2.left * moveSpeed * Time.deltaTime;
    }

    public void KillBall()
    {
        animator.SetTrigger("Break");
        rBody2D.simulated = false;
        Invoke("Break", 0.2f);
    }

    public void Break()
    {
        int amountToSpawn = Random.Range(1, 2);

        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector2 spawnPos = new Vector2(Random.value, Random.value);
            GameObject snowballInstance = Instantiate(Resources.Load("Prefabs/Items/Snowball_Pickup", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            snowballInstance.GetComponent<Rigidbody2D>().AddForce(spawnPos * 2f, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }
}
