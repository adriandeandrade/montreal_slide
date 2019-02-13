using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBird : MonoBehaviour
{
    private BirdNew bird;

    private void Start()
    {
        bird = GetComponentInParent<BirdNew>();
    }

    public void Disable()
    {
        GetComponentInParent<Transform>().gameObject.SetActive(false);
    }
}
