using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [Header("Flock Setup")]
    [Range(1, 5)] [SerializeField] private int amountOfBirds;
    [Tooltip("How far away the birds can fly from the center of their flock.")]
    public float maxIdleFlyDistance;
    [Tooltip("How fast the birds dive.")]
    [SerializeField] private float attackSpeed;
    [Tooltip("How fast the birds more when idle.")]
    [SerializeField] private float idleSpeed;
    [Tooltip("The time between dives.")]
    [SerializeField] private float timeBetweenDives;

    [HideInInspector] public bool attackHasStarted = false;

    private CircleCollider2D idleFlyRadius;
    private AudioSource flockAudio;

    [HideInInspector] public List<Bird> birds = new List<Bird>();

    private void Awake()
    {
        idleFlyRadius = GetComponent<CircleCollider2D>();
        flockAudio = GetComponent<AudioSource>();
        InitializeFlock();
    }

    private void InitializeFlock()
    {
        UpdateFlyRadius();

        for (int i = 0; i < amountOfBirds; i++)
        {
            GameObject birdInstance = Instantiate(Resources.Load("Prefabs/Entites/Entity_Bird", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            birdInstance.transform.SetParent(transform);
            birdInstance.GetComponent<Bird>().flockManager = this;
            birdInstance.GetComponent<Bird>().flockBounds = idleFlyRadius;
            birdInstance.GetComponent<Bird>().AttackSpeed = attackSpeed;
            birdInstance.GetComponent<Bird>().IdleSpeed = idleSpeed;
            birds.Add(birdInstance.GetComponent<Bird>());
        }
    }

    public IEnumerator Attack()
    {
        foreach (Bird bird in birds)
        {
            if(bird != null)
            {
                bird.GetComponentInChildren<Animator>().SetBool("isAttacking", true);
                bird.IsIdle = false;
                yield return new WaitForSecondsRealtime(timeBetweenDives);
                //bird.Launch(); // Now launches from animation event.
            }
        }

        attackHasStarted = false;
        birds.Clear();
        flockAudio.Stop();
    }

    private void UpdateFlyRadius()
    {
        idleFlyRadius.radius = maxIdleFlyDistance;
    }

    private void OnValidate()
    {
        idleFlyRadius = GetComponent<CircleCollider2D>();
        UpdateFlyRadius();
    }
}
