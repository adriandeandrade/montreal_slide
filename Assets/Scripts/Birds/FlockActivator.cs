using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockActivator : MonoBehaviour
{
    [SerializeField] private FlockManager flockManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !flockManager.attackHasStarted)
        {
            Debug.Log("START");

            StartCoroutine(flockManager.Attack());
            flockManager.attackHasStarted = true;
        }
    }
}
