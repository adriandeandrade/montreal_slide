using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOld2 : MonoBehaviour
{
    [Header("Player Fields")]
    [SerializeField] private JumpMeter jumpMeter;
    [SerializeField] private PlayerShooting shooting;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject jumpMeterUI;
    [SerializeField] private GameObject shieldObject;
     [SerializeField] private float moveSpeed;
    [SerializeField] private float movementSmoothingAmount = .05f;
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackTime;
    [SerializeField] private int currentSnowballs;
    [SerializeField] private Color hurtColor;
    [SerializeField] private Color blueColor;
    private float knockBackCounter;

    private float xMove = 0f;
    private float jumpAmount;
    private bool isGrounded;
    private bool isDamaged;
    [HideInInspector] public bool isPickingUp;
    private bool facingRight = false;
    private bool isJumping;
    public bool isThrowing;
    [HideInInspector] public int maxSnowballs = 5;

    private Vector2 Velocity;
    private HashSet<GameObject> takenDamageFrom = new HashSet<GameObject>();

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rBody2D;
    private HealthManager healthManager;

    [Header("Collision Fields")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private const float groundedRadius = 0.2f;
    public UnityEvent OnLandEvent;
    public UnityEvent OnGetShield;


    public int CurrentSnowballs
    {
        get
        {
            return currentSnowballs;
        }
        set
        {
            currentSnowballs = value;
        }
    }

    private void Awake()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        jumpMeter = GetComponent<JumpMeter>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthManager = FindObjectOfType<HealthManager>();
        shooting = GetComponent<PlayerShooting>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        if (OnGetShield == null)
        {
            OnGetShield = new UnityEvent();
        }
    }

    private void Start()
    {
        jumpMeterUI.SetActive(false);
    }

    private void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        animator.SetFloat("Speed", Mathf.Abs(xMove));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && knockBackCounter <= 0 && !isThrowing)
        {
            jumpMeterUI.SetActive(true);
            StartCoroutine(jumpMeter.CalculateJumpForce());
        }

        if (Input.GetMouseButtonDown(0) && !shooting.CoolingDown && knockBackCounter <= 0 && isGrounded && !isThrowing && currentSnowballs > 0)
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

        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (knockBackCounter <= 0)
        {
            spriteRenderer.color = Color.white;
            Move(xMove);

            if (isJumping)
            {
                rBody2D.AddForce(Vector2.up * jumpAmount * jumpHeightMultiplier, ForceMode2D.Impulse);
                isJumping = false;
            }
        }

        CheckCollisions();
    }

    private void CheckCollisions()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask); // Check for ground using ground collision.

        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != this.gameObject) // Check if we arent colliding with ourselves.
            {
                isGrounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    public void Move(float move)
    {
        Vector2 targetVelocity = new Vector2(move * 10f, rBody2D.velocity.y);
        rBody2D.velocity = Vector2.SmoothDamp(rBody2D.velocity, targetVelocity, ref Velocity, movementSmoothingAmount);

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = facingRight;
        //Vector2 xScale = transform.localScale;
        //xScale.x *= -1;
        //transform.localScale = xScale;

    }

    public void Jump(float amount)
    {
        jumpMeterUI.SetActive(false);
        jumpAmount = amount;
        isJumping = true;
        animator.SetBool("IsJumping", true);
    }

    public void OnLanding()
    {
        isJumping = false;
        animator.SetBool("IsJumping", false);
        animator.SetBool("GotHurt", false);
    }

    public void OnShield()
    {
        shieldObject.SetActive(true);
    }

    public void Knockback(Vector2 direction, Color color)
    {
        knockBackCounter = knockBackTime;
        rBody2D.velocity = -direction * knockBackForce;
        rBody2D.velocity = new Vector2(rBody2D.velocity.x, knockBackForce);

        isJumping = false;
        animator.SetBool("IsJumping", false);
        animator.SetBool("GotHurt", true);
        BlipPlayer(color);
    }

    private void BlipPlayer(Color color)
    {
        spriteRenderer.color = color;
    }

    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        if (inventory.HasShield)
        {
            inventory.HasShield = false;
            Knockback(-hitDirection, blueColor);
            shieldObject.GetComponent<Animator>().SetTrigger("ShieldBreak");
            Debug.Log("Took no damage");
            return;
        }

        if (!inventory.HasShield)
        {
            Knockback(-hitDirection, hurtColor);
            healthManager.LoseHealth(amount);
            Debug.Log("Took damage");
            return;
        }
    }

    public void TakeDamage(int amount)
    {
        if (inventory.HasShield)
        {
            inventory.HasShield = false;
            shieldObject.GetComponent<Animator>().SetTrigger("ShieldBreak");
            Debug.Log("Took no damage");
        }
        else
        {
            healthManager.LoseHealth(amount);
            Debug.Log("Took damage");
        }
    }

    public void ThrowSnowball()
    {
        shooting.Shoot();
        isThrowing = true;
    }

    public void OnFinishedThrowing()
    {
        animator.SetBool("Throwing", false);
        isThrowing = false;
    }

    private bool MouseOnLeft()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
        {
            return false;
        }
        else if (mousePos.x < transform.position.x)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 hitDirection = transform.position - other.transform.position;
        hitDirection = hitDirection.normalized;

        if (other.CompareTag("GiantSnowballInteract"))
        {
            Jump(0.8f);
            other.GetComponentInParent<GiantSnowball>().KillBall();
        }
        else if (other.CompareTag("GiantSnowball") && !isDamaged)
        {
            other.GetComponentInParent<GiantSnowball>().Knockback(hitDirection); // Apply knockback effect to giant snowball.
            TakeDamage(1, hitDirection);
            isDamaged = true;
        }
        else if (other.CompareTag("Bird") && !isDamaged)
        {
            TakeDamage(1, hitDirection);
            isDamaged = true;
        }

        if (other.CompareTag("PlayerInteract"))
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
        if (other.CompareTag("GiantSnowball") && isDamaged)
        {
            isDamaged = false;
        }
        else if (other.CompareTag("Bird") && isDamaged)
        {
            isDamaged = false;
        }

        if (other.CompareTag("PlayerInteract") && isPickingUp)
        {
            isPickingUp = false;
        }
    }
}
