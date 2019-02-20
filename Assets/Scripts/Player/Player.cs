using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Knockback))]
public class Player : MonoBehaviour, IDamageable
{
    public GameObject shieldUI;
    [SerializeField] private Color shieldBreakColor;
    [SerializeField] private Color damageColor;

    PlayerShooting playerShooting;
    PlayerMovement playerMovement;
    GameManager gameManager;
    JumpMeter jumpMeter;
    Inventory inventory;
    HealthManager healthManager;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Knockback knockback;

    Color blipColor;

    public UnityEvent OnGetShield;
    public UnityEvent OnDie;

    [HideInInspector] public bool isPickingUp;
    public bool isThrowing;
    [HideInInspector] public bool interacting;
    public bool isGettingDamaged;
    //public bool knockback;

    private void Awake()
    {
        playerShooting = GetComponent<PlayerShooting>();
        playerMovement = GetComponent<PlayerMovement>();
        gameManager = FindObjectOfType<GameManager>();
        knockback = GetComponent<Knockback>();
        inventory = FindObjectOfType<Inventory>();
        jumpMeter = GetComponent<JumpMeter>();
        healthManager = FindObjectOfType<HealthManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (OnGetShield == null)
        {
            OnGetShield = new UnityEvent();
        }

        if (OnDie == null)
        {
            OnDie = new UnityEvent();
        }
    }

    private void Start()
    {
        animator.SetBool("IsDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !playerShooting.CoolingDown && !isThrowing && Inventory.instance.CurrentSnowballs > 0 && !knockback.isKnockback)
        {
            ThrowSnowball();
        }

        if(transform.position.y < -20)
        {
            Die();
        }
    }

    private void ThrowSnowball()
    {

        if (Inventory.instance.CurrentSnowballs <= 0) return;

        if (!playerShooting.CoolingDown && !isThrowing && !knockback.isKnockback)
        {
            PlayerThrowDirection();
            Throw();
        }
        else
        {
            return;
        }
    }

    private void Throw()
    {
        if (playerMovement.isJumping)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("Throwing", true);
        }
        else
        {
            animator.SetBool("Throwing", true);
        }

        isThrowing = true;
        playerShooting.Shoot();
        AudioManager.instance.Play("player_throw");
    }

    public void OnFinishedThrowing()
    {
        animator.SetBool("Throwing", false);
        isThrowing = false;
    }

    public void TakeDamage(int amount, Vector2 direction)
    {
        if (Inventory.instance.HasShield)
        {
            blipColor = shieldBreakColor;
        }
        else
        {
            blipColor = damageColor;
        }

        knockback.ApplyKnockback(direction, blipColor);
        playerMovement.isJumping = false;
        animator.SetBool("IsJumping", false);

        isGettingDamaged = false;

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
            AudioManager.instance.Play("player_take_damage");
        }
    }

    public void Die()
    {
        animator.SetBool("IsDead", true);
        Invoke("OnDieEvent", 0.5f);
        Time.timeScale = 0.5f;
    }

    public void OnDieEvent()
    {
        OnDie.Invoke();
    }

    public void OnShield()
    {
        shieldUI.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BreakSnowball") && !interacting && !isGettingDamaged)
        {
            playerMovement.CalculateJump(0.5f);
            other.GetComponentInParent<GiantSnowball>().KillBall();
            interacting = true;
        }

        if (other.CompareTag("Enemy") && !isGettingDamaged)
        {
            if (jumpMeter.isCalculatingJump)
            {
                jumpMeter.StopCalculatingJump();
            }

            Vector2 dir = transform.position - other.transform.position;

            if (other.GetComponent<Bird>())
            {
                other.GetComponent<Collider2D>().enabled = false;
                other.GetComponent<Bird>().TakeDamage(1, Vector2.zero);
                Debug.Log("damaged");
            }

            isGettingDamaged = true;
            TakeDamage(1, dir);
        }

        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                isPickingUp = true;
                item.Init();
                Destroy(other.gameObject);
                FindObjectOfType<AudioManager>().Play("player_pickup");
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


    private void PlayerThrowDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x && !playerMovement.facingRight)
        {
            Flip();
        }
        else if (mousePos.x < transform.position.x && playerMovement.facingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        playerMovement.facingRight = !playerMovement.facingRight;
        spriteRenderer.flipX = playerMovement.facingRight;
    }
}
