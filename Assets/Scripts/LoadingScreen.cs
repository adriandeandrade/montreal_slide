using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    LevelFader levelFader;

    private void Awake()
    {
       
    }

    private void Start()
    {
        levelFader = FindObjectOfType<LevelFader>();
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(5f);
        levelFader.FadeToLevel(2);
        AudioManager.instance.Play("GameTheme");

    }
}
