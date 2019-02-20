using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSnowballSpawner : MonoBehaviour
{
    public int amountToSpawn;
    public Transform spawnPosition;
    public int timeBetweenSpawns;

    public GameObject giantSnowballPrefab;

    bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(SpawnSnowballs());
        }
    }

    IEnumerator SpawnSnowballs()
    {
        for(int i = 0; i < amountToSpawn; i++)
        {
            Instantiate(giantSnowballPrefab, spawnPosition.position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        yield break;
    }
}
