using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private LevelFader levelFader;
    [SerializeField] private GameObject helpMenu;

    private void Awake()
    {
        levelFader = FindObjectOfType<LevelFader>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            helpMenu.SetActive(false);
        }
    }

    public void Play()
    {
        levelFader.FadeToLevel(1);
    }

    public void Help()
    {
        helpMenu.SetActive(true);
    }

    public void ExitHelpMenu()
    {
        helpMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
