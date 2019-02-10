using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnHeartKilled()
    {
        animator.SetBool("isAlive", false);
    }

    public void HeartAlive()
    {
        animator.SetBool("isAlive", true);
    }
}
