using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    private GameObject snowballHitEffect;

    private void Start()
    {
        snowballHitEffect = FindObjectOfType<GameManager>().snowballHitEffect;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject hitEffect = Instantiate(snowballHitEffect, other.contacts[0].point, Quaternion.identity);
        Destroy(hitEffect, 3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bird"))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                //damageable.TakeDamage(1);
                GameObject hitEffect = Instantiate(snowballHitEffect, other.transform.position, Quaternion.identity);
                Destroy(hitEffect, 3f);
                Destroy(gameObject);
            }
        }
    }
}
