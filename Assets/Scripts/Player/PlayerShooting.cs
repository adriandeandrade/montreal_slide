using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float throwCooldown;

    [SerializeField] private Canvas cooldownBarUI;
    [SerializeField] private GameObject snowBallPrefab;
    public Image cooldownBarImage;
    [SerializeField] private TextMeshProUGUI snowballAmountText;

    [SerializeField] private float snowballSpeed;

    Inventory inventory;
    Animator animator;

    float currentCooldown;
    float cooldownRatio;
    bool coolingDown = false;

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
        inventory = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        currentCooldown = throwCooldown;
    }

    private void Update()
    {
        snowballAmountText.text = "* " + inventory.CurrentSnowballs.ToString();

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
        cooldownRatio = currentCooldown / throwCooldown;
        cooldownBarUI.enabled = true;
        cooldownBarImage.fillAmount = cooldownRatio;
        currentCooldown -= Time.deltaTime;
    }

    private void ResetCooldown()
    {
        coolingDown = false;
        cooldownBarUI.enabled = false;
        currentCooldown = throwCooldown;
        cooldownBarImage.fillAmount = 0;
    }

    public void Shoot()
    {
        coolingDown = true;
        inventory.CurrentSnowballs--;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get direction
        Vector3 shotDirection = mousePos - transform.position;
        shotDirection.z = 0;

        Quaternion rotation = Quaternion.LookRotation(shotDirection, Vector3.left); // Rotate sprite to face that direction
        rotation.eulerAngles = new Vector3(0f, 0f, rotation.eulerAngles.z);

        GameObject snowballInstance = Instantiate(snowBallPrefab, transform.position, rotation); // Spawn snowball
        snowballInstance.GetComponent<Rigidbody2D>().velocity = shotDirection.normalized * snowballSpeed;
        Destroy(snowballInstance, 3f);
    }
}
