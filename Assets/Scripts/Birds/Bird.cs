using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float diveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private Vector2 moveDirection;

    [SerializeField] private Transform target;
    [SerializeField] private CircleCollider2D flockRange;
    [SerializeField] private Transform flock;
    private Animator animator;
    private SpriteRenderer sr;

    private bool hasTarget;
    private bool attacking;
    private bool facingRight = false;

    [SerializeField] private float timeUntilNextDirection = 3f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        attacking = false;
    }

    private void Start()
    {
        flock = GetComponentInParent<Transform>();
        Flip();
        moveDirection = ChangeDirection();
        StartCoroutine(Move());
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, Vector2.left * attackRange, Color.white);

        if (moveDirection.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveDirection.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        sr.flipX = facingRight;

    }

    IEnumerator Move()
    {
        while (!hasTarget)
        {
            if (Vector2.Distance(transform.position, target.transform.position) >= attackRange)
            {
                hasTarget = true;
                StartCoroutine(Attack());
                break;
            }

            if (Vector2.Distance(flock.position, transform.position) > 5)
            {
                moveDirection = -moveDirection;
            }

            else if (timeUntilNextDirection <= 0)
            {
                moveDirection = ChangeDirection();
                timeUntilNextDirection = 3f;
            }

            transform.Translate(moveDirection.normalized * diveSpeed * Time.deltaTime);

            timeUntilNextDirection -= Time.deltaTime; ;
        }

        yield return null;
    }

    IEnumerator Attack()
    {
        attacking = true;
        animator.SetBool("hasTarget", true);
        yield return new WaitForSeconds(0.8f);

        Vector2 newDirection = target.transform.position - transform.position;
        moveDirection = newDirection.normalized;

        while (attacking)
        {
            transform.Translate(moveDirection.normalized * diveSpeed * Time.deltaTime);
        }
    }

    IEnumerator ReturnToFlock()
    {
        while (Vector2.Distance(flock.position, transform.position) < 1f)
        {
            Vector2 newDirection = flock.position - transform.position;
            moveDirection = newDirection.normalized;
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Vector2 hitDirection = transform.position - other.transform.position;
            hitDirection = hitDirection.normalized;
            attacking = false;
            //other.GetComponent<IDamageable>().TakeDamage(1, hitDirection);
        }
    }

    private Vector2 ChangeDirection()
    {
        Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        return newDirection.normalized;

    }

    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        animator.SetBool("isDead", true);
        Destroy(gameObject, 0.5f);
    }

    public void TakeDamage(int amount)
    {
        animator.SetBool("isDead", true);
        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position * flockRange.radius);
    }
}
