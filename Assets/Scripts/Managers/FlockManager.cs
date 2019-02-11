using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public CircleCollider2D col;
    private Transform player;
    public bool picked = false;
    [SerializeField] private float attackDistance;

    public List<Bird> birds = new List<Bird>();

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromFlock = Vector2.Distance(player.position, transform.position);

        if (distanceFromFlock <= col.radius + attackDistance && !picked)
        {
            picked = true;
            int pickRandomBird = Random.Range(1, birds.Count);
            if(!birds[pickRandomBird].hasAttacked)
            {
                birds[pickRandomBird].states = Bird.BIRD_STATE.ATTACK;
                birds[pickRandomBird].StartCoroutine(birds[pickRandomBird].states.ToString());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, col.radius);
    }
}
