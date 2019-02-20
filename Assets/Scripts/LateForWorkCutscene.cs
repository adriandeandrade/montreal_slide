using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateForWorkCutscene : MonoBehaviour
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
        levelFader.FadeToLevel(3);
        //AudioManager.instance.Play("GameTheme");
        yield break;

    }
}
