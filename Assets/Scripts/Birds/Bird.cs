using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour, IDamageable
{
    [Header("Bird Setup")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float idleSpeed;
    private GameObject target;
    public CircleCollider2D flockBounds;
    public FlockManager flockManager;

    private float timeUntilPickNextDirection = 3f;
    private bool isIdle = true;
    private bool facingRight;
    private bool tooFarFromFlock;
    private bool launch;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rBody;
    private Vector3 movement;

    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
        set
        {
            attackSpeed = value;
        }
    }
    public float IdleSpeed
    {
        get
        {
            return idleSpeed;
        }
        set
        {
            idleSpeed = value;
        }
    }
    public bool IsIdle
    {
        get
        {
            return isIdle;
        }
        set
        {
            isIdle = value;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<PlayerMovement>().gameObject;

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

        if (launch)
        {
            transform.Translate(movement.normalized * attackSpeed * Time.deltaTime);
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
        AudioManager.instance.Play("bird_attack");
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
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            TakeDamage(1, Vector2.zero);
        }

        if (other.CompareTag("Flock"))
        {
            tooFarFromFlock = false;
        }
        else if (other.CompareTag("KillZone"))
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int amount, Vector2 direction)
    {
        animator.SetTrigger("hasDied");
        // Object will get deleted with an animatione event.
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Flock"))
        {
            tooFarFromFlock = true;
        }
    }
}
