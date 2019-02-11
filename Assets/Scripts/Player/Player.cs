using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : BaseEntity
{
    [Header("Player Setup")]
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private Color shieldBreakColor;
    public UnityEvent OnGetShield;

    [Header("Player Components")]
    [SerializeField] private JumpMeter jumpMeter;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private Inventory inventory;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private GameObject jumpMeterUI;
    [SerializeField] private GameObject shieldUI;

    [HideInInspector] public bool isPickingUp;
    private bool isThrowing;
    private float jumpAmount;

    protected override void Awake()
    {
        base.Awake();

        jumpMeter = GetComponent<JumpMeter>();
        playerShooting = GetComponent<PlayerShooting>();
        healthManager = FindObjectOfType<HealthManager>();

        if (OnGetShield == null)
        {
            OnGetShield = new UnityEvent();
        }
    }

    private void Start()
    {
        jumpMeterUI.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        xMove = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f);
        animator.SetFloat("Speed", Mathf.Abs(xMove.x));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !knockback && !isThrowing)
        {
            jumpMeterUI.SetActive(true);
            StartCoroutine(jumpMeter.CalculateJumpForce());
        }


        if (Input.GetMouseButtonDown(0) && !playerShooting.CoolingDown && knockBackCounter <= 0 && isGrounded && !isThrowing && inventory.CurrentSnowballs > 0)
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
    }

    public void OnShield()
    {
        shieldUI.SetActive(true);
    }

    public override void Knockback(Transform other, Color color)
    {
        base.Knockback(other, color);
        isJumping = false;
        animator.SetBool("IsJumping", false);
        animator.SetBool("GotHurt", true);
    }

    public override void TakeDamage(int amount, Transform objectHit)
    {
        Knockback(objectHit, damageBlipColor);
        healthManager.LoseHealth(amount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isGettingDamaged)
        {
            if (inventory.HasShield)
            {
                other.GetComponentInParent<NewGiantSnowball>().Knockback(transform, Color.white);
                Knockback(other.transform, shieldBreakColor);
                inventory.HasShield = false;
                shieldUI.GetComponent<Animator>().SetTrigger("ShieldBreak");
                isGettingDamaged = true;
            }
            else if (!inventory.HasShield)
            {
                other.GetComponentInParent<NewGiantSnowball>().Knockback(transform, Color.white);
                TakeDamage(1, other.transform);
                isGettingDamaged = true;
            }


        }
        else if (other.CompareTag("GiantSnowballInteract"))
        {
            Jump(1f);
            other.GetComponentInParent<NewGiantSnowball>().KillBall();
        }
        else if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null && !isPickingUp)
            {
                item.Init();
                Destroy(other.gameObject);
                isPickingUp = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && isGettingDamaged)
        {
            isGettingDamaged = false;
        }
        else if (other.CompareTag("Bird") && isGettingDamaged)
        {
            isGettingDamaged = false;
        }

        if (other.CompareTag("Item") && isPickingUp)
        {
            isPickingUp = false;
        }
    }

}
