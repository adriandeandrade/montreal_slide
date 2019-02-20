using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutscene : MonoBehaviour
{
    LevelFader levelFader;

    private void Start()
    {
        levelFader = FindObjectOfType<LevelFader>();
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(5f);
        levelFader.FadeToLevel(4);
        //AudioManager.instance.Play("GameTheme");
        yield break;

    }
}
