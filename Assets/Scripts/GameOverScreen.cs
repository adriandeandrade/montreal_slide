using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    

    public void Respawn()
    {
        Time.timeScale = 1f;
        gameManager.Respawn();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        gameManager.GoToMainMenu();
    }
}
