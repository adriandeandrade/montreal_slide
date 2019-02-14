using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkSounds : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayWalkOne()
    {
        audioManager.Play("player_walk_one");
    }

    public void PlayWalkTwo()
    {
        audioManager.Play("player_walk_two");
    }
}
