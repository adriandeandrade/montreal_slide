using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Particle System Prefabs")]
    public GameObject snowballHitEffect;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverButtons;

    bool gameOver;

    LevelFader levelFader;
    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        levelFader = FindObjectOfType<LevelFader>();
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            Debug.Log("Sequence Started");
            gameOver = true;
            StartCoroutine(GameOverSequence());
        }
    }

    IEnumerator GameOverSequence()
    {
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        gameOverButtons.SetActive(true);
        Cursor.visible = true;
        yield break;
    }

    public void GoToMainMenu()
    {
        levelFader.FadeToLevel(0);
    }

    public void Respawn()
    {
        levelFader.FadeToLevel(SceneManager.GetActiveScene().buildIndex);
    }
}
