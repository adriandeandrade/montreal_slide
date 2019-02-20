using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float timerAmount;

    GameManager gameManager;

    float timeLeft;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        timeLeft = timerAmount;
    }


    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = "Time Left: " + timeLeft.ToString();

        if(timeLeft < 0)
        {
            gameManager.GameOver();
        }
    }

    public void AddTime(float amount)
    {
        timeLeft += amount;
    }

}
