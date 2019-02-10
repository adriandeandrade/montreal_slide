using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Canvas cooldownBarUI;
    public Image cooldownBarImage;
    [SerializeField] private GameObject snowBallPrefab;
    [SerializeField] private float snowballSpeed;

    public float cooldown;
    private float currentCooldown;
    private float cooldownRatio;
    private bool coolingDown = false;

    public bool CoolingDown
    {
        get
        {
            return coolingDown;
        }
        set
        {
            coolingDown = value;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentCooldown = cooldown;
    }

    private void Update()
    {
        if (coolingDown)
        {
            UpdateCooldownBar();
        }

        if (currentCooldown <= 0)
        {
            ResetCooldown();
        }
    }

    private void UpdateCooldownBar()
    {
        cooldownRatio = currentCooldown / cooldown;
        cooldownBarUI.enabled = true;
        cooldownBarImage.fillAmount = cooldownRatio;
        currentCooldown -= Time.deltaTime;
    }

    private void ResetCooldown()
    {
        coolingDown = false;
        currentCooldown = cooldown;
        cooldownBarUI.enabled = false;
        cooldownBarImage.fillAmount = 0;
    }

    public void Shoot()
    {
        if (!coolingDown)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 shotDirection = mousePos - transform.position;
            shotDirection.z = 0;
            Quaternion rotation = Quaternion.LookRotation(shotDirection, Vector3.left);
            rotation.eulerAngles = new Vector3(0f, 0f, rotation.eulerAngles.z);
            GetComponent<Player>().CurrentSnowballs--;
            GameObject snowballInstance = Instantiate(snowBallPrefab, transform.position, rotation);
            snowballInstance.GetComponent<Rigidbody2D>().velocity = shotDirection.normalized * snowballSpeed; //new Vector2(shotDirection.x * snowballSpeed, shotDirection.y * snowballSpeed);
            Destroy(snowballInstance, 3f);

            coolingDown = true;
        }
    }
}
