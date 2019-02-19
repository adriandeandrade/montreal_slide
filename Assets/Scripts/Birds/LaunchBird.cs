using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBird : MonoBehaviour
{
    private Bird bird;

    private void Start()
    {
        bird = GetComponentInParent<Bird>();
    }

    public void Disable()
    {
        GetComponentInParent<Transform>().gameObject.SetActive(false);
    }
}
