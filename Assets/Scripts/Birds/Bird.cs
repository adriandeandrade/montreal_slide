using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : BaseEntity
{
    public enum BIRD_STATE { IDLE = 0, ATTACK = 1, RESET = 2 }

    [Header("Bird Setup")]
    public BIRD_STATE states;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private FlockManager flockManager;

    [SerializeField] private CircleCollider2D flockCollider;

    private bool tooFarFromFlock;
    private float timeUntilPickNextDirection = 3f;

    private Vector3 startPos;
    private Vector2 movement;

    protected override void Awake()
    {
        base.Awake();
        states = BIRD_STATE.IDLE;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        target = FindObjectOfType<Player>().transform;
        flockManager = GetComponentInParent<FlockManager>();
        StartCoroutine(EnemyFSM());
        startPos = transform.position;
        movement = PickNewDirection();
    }

    protected override void Update()
    {
        HandleKnockback();
        if(!knockback)
        {
            Move();
        }
    }

    private void OnDestroy()
    {
        flockManager.hasPickedBirdToAttack = false;
    }

    protected override void Move()
    {
        Vector2 targetVelocity = new Vector2(movement.x, movement.y);
        //rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, moveSpeedSmoothing);
        transform.Translate(targetVelocity * moveSpeed * Time.deltaTime);

        if (movement.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && facingRight)
        {
            Flip();
        }
    }

    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(states.ToString());
        }
    }

    IEnumerator IDLE()
    {
        Debug.Log("IDLEING");
        animator.SetBool("isFlying", true);
        while (states == BIRD_STATE.IDLE)
        {
            if (timeUntilPickNextDirection <= 0 || tooFarFromFlock)
            {
                movement = PickNewDirection();
                timeUntilPickNextDirection = 3f;
            }

            timeUntilPickNextDirection -= Time.deltaTime;
            yield return new WaitForSeconds(0f);
        }
    }

    IEnumerator ATTACK()
    {
        Debug.Log("ATTACKING");
        animator.SetTrigger("targetAquired");
        yield return new WaitForSeconds(0.5f);
        Vector2 targetPos = target.position - transform.position;
        moveSpeed = attackSpeed;
        movement = targetPos.normalized;

        while (states == BIRD_STATE.ATTACK)
        {
            yield return new WaitForSeconds(.8f);
            states = BIRD_STATE.RESET;
            yield return StartCoroutine(states.ToString());
        }
    }

    IEnumerator RESET()
    {
        Debug.Log("RESETTING");
        moveSpeed = 10f;
        while (states == BIRD_STATE.RESET)
        {
            movement = (startPos - transform.position).normalized;
            yield return new WaitForSeconds(0f);
        }
    }

    private Vector2 PickNewDirection()
    {
        Vector2 myPos = transform.position;
        Vector2 flockPos = flockManager.transform.position;
        Vector2 newDir = (flockPos + Random.insideUnitCircle * flockManager.maxIdleFlyDistance) - myPos;
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
            states = BIRD_STATE.RESET;
            StartCoroutine(states.ToString());
        }

        if(other.CompareTag("BirdReset") && states == BIRD_STATE.RESET)
        {
            Debug.Log("has reset");
            flockManager.hasPickedBirdToAttack = false;
            states = BIRD_STATE.IDLE;
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
