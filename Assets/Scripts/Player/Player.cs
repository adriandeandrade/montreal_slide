using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxSnowballs = 5;
    [SerializeField] private int currentSnowballs;
    [SerializeField] private int maxHealth = 3;

    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    private PlayerShooting shooting;

    private void Start()
    {
        shooting = GetComponent<PlayerShooting>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && HasAmmo()) // Left mouse button
        {
            shooting.Shoot();
            currentSnowballs -= 1;
        }
    }

    private bool HasAmmo()
    {
        if (currentSnowballs > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(int amount)
    {
        if(Health > 0)
        {

        }
    }
}
