using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdNew : MonoBehaviour, IDamageable
{
    [Header("Bird Setup")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float idleSpeed;
    [SerializeField] private GameObject target;
    public CircleCollider2D flockBounds;
    public FlockManager flockManager;

    [HideInInspector] public bool isIdle = true;
    private bool facingRight;
    private bool tooFarFromFlock;
    private bool launch;
    private float timeUntilPickNextDirection = 3f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 movement;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        target = FindObjectOfType<Player>().gameObject;

        movement = PickNewDirection();
    }

    private void Update()
    {
        if (isIdle)
        {
            Move();

            if (timeUntilPickNextDirection <= 0 || tooFarFromFlock)
            {
                movement = PickNewDirection();
                timeUntilPickNextDirection = 3f;
            }
            else
            {
                timeUntilPickNextDirection -= Time.deltaTime;
            }
        }

        if(launch)
        {
            //Vector3 direction = target.transform.position - transform.position;
            transform.Translate(movement * attackSpeed * Time.deltaTime);
        }
    }

    private void Move()
    {
        Vector2 targetVelocity = new Vector2(movement.x, movement.y);
        transform.Translate(targetVelocity * idleSpeed * Time.deltaTime);

        if (movement.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && facingRight)
        {
            Flip();
        }
    }

    public void Launch()
    {
        launch = true;
        movement = target.transform.position - transform.position;
    }

    protected void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = facingRight;
    }

    private Vector2 PickNewDirection()
    {
        Vector2 myPos = transform.position;
        Vector2 flockPos = flockManager.transform.position;
        Vector2 newDir = (flockPos + Random.insideUnitCircle * flockManager.maxIdleFlyDistance) - myPos;
        return newDir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Flock"))
        {
            tooFarFromFlock = false;
        } else if(other.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        } else if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount, Transform objectHit)
    {
        animator.SetTrigger("hasDied");
        gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Flock"))
        {
            tooFarFromFlock = true;
        }
    }

}
