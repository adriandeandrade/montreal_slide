using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverButton : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void OnHover()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
