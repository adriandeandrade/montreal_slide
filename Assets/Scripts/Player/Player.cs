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
    Inventory inventory;
    HealthManager healthManager;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Knockback knockback;

    public UnityEvent OnGetShield;

    [HideInInspector] public bool isPickingUp;
    [HideInInspector] public bool isThrowing;
    [HideInInspector] public bool interacting;
    [HideInInspector] public bool isGettingDamaged;
    //public bool knockback;

    private void Awake()
    {
        playerShooting = GetComponent<PlayerShooting>();
        playerMovement = GetComponent<PlayerMovement>();
        knockback = GetComponent<Knockback>();
        inventory = FindObjectOfType<Inventory>();
        healthManager = FindObjectOfType<HealthManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (OnGetShield == null)
        {
            OnGetShield = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !playerShooting.CoolingDown && !isThrowing && Inventory.instance.CurrentSnowballs > 0 && !knockback.isKnockback & !playerMovement.isJumping)
        {
            if (MouseOnLeft() && playerMovement.facingRight)
            {
                Flip();
                animator.SetBool("Throwing", true);
                ThrowSnowball();
            }
            else if (!MouseOnLeft() && !playerMovement.facingRight)
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

    public void ThrowSnowball()
    {
        if (!playerMovement.isGrounded)
        {
            playerShooting.Shoot();
            isThrowing = false;
            animator.SetBool("Throwing", false);
            AudioManager.instance.Play("player_throw");
        }
        else
        {
            playerShooting.Shoot();
            AudioManager.instance.Play("player_throw");
            isThrowing = true;
        }
    }

    public void OnFinishedThrowing()
    {
        animator.SetBool("Throwing", false);
        isThrowing = false;
    }

    public void TakeDamage(int amount, Vector2 direction)
    {
        knockback.ApplyKnockback(direction, damageColor);
        playerMovement.isJumping = false;
        animator.SetBool("IsJumping", false);

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

    public void OnShield()
    {
        shieldUI.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BreakSnowball") && !interacting)
        {
            playerMovement.CalculateJump(0.5f);
            other.GetComponentInParent<GiantSnowball>().KillBall();
            interacting = true;
        }

        if (other.CompareTag("Enemy") && !isGettingDamaged)
        {
            Vector2 dir = transform.position - other.transform.position;
            TakeDamage(1, dir);
            if (other.GetComponent<BirdNew>())
            {
                other.GetComponent<BirdNew>().TakeDamage(1, Vector2.zero);
            }
            isGettingDamaged = true;
        }

        if (other.CompareTag("Item") && !isPickingUp)
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

    public void Flip()
    {
        playerMovement.facingRight = !playerMovement.facingRight;
        spriteRenderer.flipX = playerMovement.facingRight;
    }
}
