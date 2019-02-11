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

    [HideInInspector] public bool hasPickedBirdToAttack = false;

    [SerializeField] private CircleCollider2D idleFlyRadius;
    private Transform player;
    [HideInInspector] public List<Bird> birds = new List<Bird>();

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
        idleFlyRadius = GetComponent<CircleCollider2D>();
        InitializeFlock();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void InitializeFlock()
    {
        idleFlyRadius.radius = maxIdleFlyDistance;

        for (int i = 0; i < amountOfBirds; i++)
        {
            GameObject birdInstance = Instantiate(Resources.Load("Prefabs/Entites/Entity_Bird", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
            birdInstance.transform.SetParent(transform);
            birds.Add(birdInstance.GetComponent<Bird>());
        }
    }

    void Update()
    {
        if(birds.Count > 0)
        {
            float distanceFromFlock = Vector2.Distance(player.position, transform.position);

            if (distanceFromFlock <= idleFlyRadius.radius + attackDistance && !hasPickedBirdToAttack)
            {
                hasPickedBirdToAttack = true;
                int pickRandomBird = Random.Range(0, birds.Count);
                birds[pickRandomBird].states = Bird.BIRD_STATE.ATTACK;
                birds[pickRandomBird].StartCoroutine(birds[pickRandomBird].states.ToString());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, idleFlyRadius.radius);
    }
}
