using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Animator animator;
    private CooldownBar cooldownBar;
    [SerializeField] private Canvas cooldownBarUI;
    [SerializeField] private GameObject snowBallPrefab;
    [SerializeField] private float snowballSpeed;

    public float cooldown;
    public float currentCooldown;
    public bool coolingDown = false;

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
            cooldownBarUI.enabled = true;
            currentCooldown -= Time.deltaTime;

            
        }

        if (currentCooldown <= 0)
        {
            coolingDown = false;
            currentCooldown = cooldown;
            cooldownBar.ResetBar();
            cooldownBarUI.enabled = false;
        }
    }

    public void Shoot()
    {
        if (!coolingDown)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 shotDirection = mousePos - transform.position;
            shotDirection.z = 0;

            GameObject snowballInstance = Instantiate(snowBallPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            snowballInstance.GetComponent<Rigidbody2D>().velocity = shotDirection.normalized * snowballSpeed; //new Vector2(shotDirection.x * snowballSpeed, shotDirection.y * snowballSpeed);
            Destroy(snowballInstance, 3f);
            
            coolingDown = true;
        }
    }
}
