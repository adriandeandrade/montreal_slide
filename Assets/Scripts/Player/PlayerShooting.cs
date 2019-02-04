using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject snowBallPrefab;
    [SerializeField] private float snowballSpeed;

    [SerializeField] private float cooldown;
    private float nextShotTime;

    public void Shoot()
    {
        if(Time.time > nextShotTime)
        {
            Vector3 shotDirection = Input.mousePosition;
            shotDirection.z = 0.0f;
            shotDirection = Camera.main.ScreenToWorldPoint(shotDirection);
            shotDirection = shotDirection - transform.position;

            GameObject snowballInstance = Instantiate(snowBallPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            snowballInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(shotDirection.x * snowballSpeed, shotDirection.y * snowballSpeed);
            Destroy(snowballInstance, 3f);

            nextShotTime = Time.time + cooldown;
        }
    }
}
