using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private int amountOfHearts;
    [SerializeField] private int currentHealth;

    public int Health
    {
        get
        {
            return currentHealth;
        }
    }

    private void Start()
    {
        amountOfHearts = 3;
        currentHealth = 3;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].GetComponent<Heart>().HeartAlive();
            hearts[i].sprite = fullHeart;
        }
    }

    private void Update()
    {
        if (currentHealth > amountOfHearts)
        {
            currentHealth = amountOfHearts;
        }

        if (currentHealth < 1)
        {
            Debug.Log("Player has died.");
        }
    }

    public void LoseHealth(int amount)
    {
        currentHealth -= 1;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].GetComponent<Heart>().HeartAlive();
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].GetComponent<Heart>().OnHeartKilled();
                hearts[i].sprite = emptyHeart;
            }

            if (i < amountOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
