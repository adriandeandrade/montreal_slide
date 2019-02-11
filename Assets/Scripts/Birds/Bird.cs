using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : BaseEntity
{
    public enum BIRD_STATE { IDLE = 0, ATTACK = 1, RESET = 2 }

    [Header("Bird Setup")]
    public BIRD_STATE states;
    [SerializeField] private float attackDistance;
    [SerializeField] private Transform target;
    [SerializeField] private FlockManager flockManager;

    private Transform flock;
    private CircleCollider2D flockCollider;

    private float timeUntilPickNextDirection = 3f;
    private bool tooFarFromFlock;
    [HideInInspector] public bool hasAttacked = false;
    private Vector3 startPos;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        flock = GetComponentInParent<FlockManager>().transform;
        flockCollider = flock.GetComponentInParent<CircleCollider2D>();
        flockManager = GetComponentInParent<FlockManager>();
        target = FindObjectOfType<Player>().transform;

        states = BIRD_STATE.IDLE;
    }

    private void Start()
    {
        StartCoroutine(EnemyFSM());
        startPos = transform.position;
        xMove = PickNewDirection() * moveSpeed * Time.deltaTime;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Move();
    }

    protected override void Move()
    {
        Vector2 targetVelocity = new Vector2(xMove.x * 10f, xMove.y * 10f);
        rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, moveSpeedSmoothing);

        if (xMove.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (xMove.x < 0 && facingRight)
        {
            Flip();
        }
    }

    IEnumerator EnemyFSM()
    {
        while (true)
        {
            Debug.Log("RUNNING");
            yield return StartCoroutine(states.ToString());
        }
    }

    IEnumerator IDLE()
    {
        while (states == BIRD_STATE.IDLE)
        {
            animator.SetBool("isFlying", true);
            if (timeUntilPickNextDirection <= 0 || tooFarFromFlock)
            {
                xMove = PickNewDirection() * moveSpeed * Time.deltaTime;
                timeUntilPickNextDirection = 3f;
            }

            timeUntilPickNextDirection -= Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }
    }

    IEnumerator ATTACK()
    {
        while (states == BIRD_STATE.ATTACK)
        {
            animator.SetTrigger("targetAquired");
            yield return new WaitForSeconds(0.3f);
            hasAttacked = true;
            Vector2 targetPos = target.position - transform.position;
            xMove = targetPos.normalized * 100 * Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }
    }

    IEnumerator RESET()
    {
        while (states == BIRD_STATE.RESET)
        {
            if (transform.position != startPos)
            {
                xMove = startPos * moveSpeed * Time.deltaTime;
            }

            hasAttacked = false;
            states = BIRD_STATE.IDLE;
            yield return StartCoroutine(states.ToString());
        }
    }

    private Vector2 PickNewDirection()
    {
        Vector2 myPos = transform.position;
        Vector2 flockPos = flock.position;
        Vector2 newDir = (flockPos + Random.insideUnitCircle * flockCollider.radius) - myPos;
        return newDir.normalized;
    }


    public override void TakeDamage(int amount, Transform objectHit)
    {
        Knockback(objectHit, damageBlipColor);
        animator.SetTrigger("hasDied");
        flockManager.birds.Remove(this);
        Destroy(gameObject, 0.8f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flock"))
        {
            tooFarFromFlock = false;
        }

        if (other.CompareTag("Player") && states == BIRD_STATE.ATTACK)
        {
            flockManager.picked = false;
            states = BIRD_STATE.RESET;
            StartCoroutine(states.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Flock"))
        {
            tooFarFromFlock = true;
        }
    }
}
