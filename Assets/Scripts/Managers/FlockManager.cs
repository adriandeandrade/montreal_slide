using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [Header("Flock Setup")]
    [Range(1, 5)] [SerializeField] private int amountOfBirds;
    [Tooltip("How far the player needs to be for the flock to be in attack mode.")]
    [SerializeField] private float attackDistance;
    [Tooltip("How far away the birds can fly from the center of their flock.")]
    public float maxIdleFlyDistance;
    [Tooltip("This is a gameobject with a collider on it which will activate the attack sequence once the players walks in it.")]
    [SerializeField] private GameObject flockActivator;

    public bool attackHasStarted = false;

    [SerializeField] private CircleCollider2D idleFlyRadius;
    private Transform player;
    public List<BirdNew> birds = new List<BirdNew>();

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
        idleFlyRadius = GetComponent<CircleCollider2D>();
        InitializeFlock();
    }

    private void InitializeFlock()
    {
        idleFlyRadius.radius = maxIdleFlyDistance;

        for (int i = 0; i < amountOfBirds; i++)
        {
            GameObject birdInstance = Instantiate(Resources.Load("Prefabs/Entites/Entity_Bird", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            birdInstance.transform.SetParent(transform);
            birdInstance.GetComponent<BirdNew>().flockManager = this;
            birdInstance.GetComponent<BirdNew>().flockBounds = idleFlyRadius;
            birds.Add(birdInstance.GetComponent<BirdNew>());
        }
    }

    IEnumerator Attack()
    {
        foreach (BirdNew bird in birds)
        {
            if(bird != null)
            {
                bird.GetComponentInChildren<Animator>().SetBool("isAttacking", true);
                yield return new WaitForSecondsRealtime(0.8f);
                bird.Launch();
            }
        }

        attackHasStarted = false;
        birds.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, idleFlyRadius.radius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !attackHasStarted)
        {
            Debug.Log("START");

            StartCoroutine(Attack());
            attackHasStarted = true;
            foreach (BirdNew bird in birds)
            {
                bird.isIdle = false;
            }
        }
    }
}
