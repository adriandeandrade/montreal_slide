using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : BaseEntity
{
    [Header("Player Setup")]
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private Color shieldBreakColor;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackTime;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip pickUpItemSound;
    [SerializeField] private AudioClip dealDamage;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip takeDamage;

    [Header("Other Player Events")]
    public UnityEvent OnGetShield;

    [Header("Player Components")]
    [SerializeField] private JumpMeter jumpMeter;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private Inventory inventory;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private GameObject jumpMeterUI;
    public GameObject shieldUI;

    public bool isPickingUp;
    private bool isThrowing;
    public bool interacting;
    private bool knockback;
    private float jumpAmount;
    public bool isGettingDamaged;

    protected override void Awake()
    {
        base.Awake();

        jumpMeter = GetComponent<JumpMeter>();
        playerShooting = GetComponent<PlayerShooting>();
        healthManager = FindObjectOfType<HealthManager>();
        inventory = Inventory.instance;

        if (OnGetShield == null)
        {
            OnGetShield = new UnityEvent();
        }
    }

    private void Start()
    {
        jumpMeterUI.SetActive(false);
    }

    protected override void Update()
    {
        if (GetComponent<Knockback>() && !knockback)
        {
            knockback = true;
        }
        else if (!GetComponent<Knockback>() && knockback)
        {
            knockback = false;
        }

        xMove = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f);
        animator.SetFloat("Speed", Mathf.Abs(xMove.x));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isThrowing && !knockback)
        {
            jumpMeterUI.SetActive(true);
            StartCoroutine(jumpMeter.CalculateJumpForce());
        }

        if (Input.GetMouseButtonDown(0) && !playerShooting.CoolingDown && isGrounded && !isThrowing && Inventory.instance.CurrentSnowballs > 0 && !knockback)
        {
            if (MouseOnLeft() && facingRight)
            {
                Flip();
                animator.SetBool("Throwing", true);
                ThrowSnowball();
            }
            else if (!MouseOnLeft() && !facingRight)
            {
                Flip();
                animator.SetBool("Throwing", true);
                ThrowSnowball();
            }
            else
            {
                animator.SetBool("Throwing", true);
                ThrowSnowball();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!knockback)
        {
            Move();
        }

        if (isJumping)
        {
            rBody2D.AddForce(Vector2.up * jumpAmount * jumpHeightMultiplier, ForceMode2D.Impulse);
            isJumping = false;
        }
    }

    public void Jump(float amount)
    {
        jumpMeterUI.SetActive(false);
        jumpAmount = amount;
        isJumping = true;
        animator.SetBool("IsJumping", true);
        audioSource.PlayOneShot(jumpSound);
    }

    public void ThrowSnowball()
    {
        playerShooting.Shoot();
        isThrowing = true;
    }

    public void OnFinishedThrowing()
    {
        animator.SetBool("Throwing", false);
        isThrowing = false;
    }

    public override void OnLanding()
    {
        base.OnLanding();
        isJumping = false;
        animator.SetBool("IsJumping", false);
        animator.SetBool("GotHurt", false);
        audioSource.PlayOneShot(landSound);
    }

    public void OnShield()
    {
        shieldUI.SetActive(true);
    }

    public override void TakeDamage(int amount, Transform objectHit)
    {
        if (!GetComponent<Knockback>())
        {
            gameObject.AddComponent<Knockback>().InitKnockback(knockbackTime, knockbackForce, objectHit);
            isJumping = false;
            animator.SetBool("IsJumping", false);
            animator.SetBool("GotHurt", true);
            audioSource.PlayOneShot(takeDamage);
        }

        if (Inventory.instance.HasShield)
        {
            Debug.Log("Took no damage because shield.");
            Inventory.instance.HasShield = false;
            shieldUI.GetComponent<Animator>().SetTrigger("ShieldBreak");
            return;
        }

        if (!Inventory.instance.HasShield)
        {
            Debug.Log("Took damage because no shield.");
            healthManager.LoseHealth(amount);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BreakSnowball") && !interacting)
        {
            Jump(1f);
            other.GetComponentInParent<GiantSnowball>().KillBall();
            interacting = true;
        }

        if (other.CompareTag("Enemy") && !isGettingDamaged)
        {
            TakeDamage(1, other.transform);
            if(other.GetComponent<BirdNew>())
            {
                other.GetComponent<BirdNew>().TakeDamage(1, transform);
            }
            isGettingDamaged = true;
        }

        if (other.CompareTag("Item") && !isPickingUp)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                item.Init();
                Destroy(other.gameObject);
                isPickingUp = true;
                audioSource.PlayOneShot(pickUpItemSound);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && isGettingDamaged)
        {
            isGettingDamaged = false;
        }

        if (other.CompareTag("Item") && isPickingUp)
        {
            isPickingUp = false;
        }

        if (other.CompareTag("BreakSnowball") && interacting)
        {
            interacting = false;
        }
    }

}
