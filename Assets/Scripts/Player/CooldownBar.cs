using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerShooting shooting;
    public Image cooldownBarImage;

    private void Awake()
    {
        shooting = FindObjectOfType<PlayerShooting>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if(shooting.coolingDown)
        {
            cooldownBarImage.fillAmount += 1.5f * Time.deltaTime;
        }
    }

    public void ResetBar()
    {
        cooldownBarImage.fillAmount = 0f;
    }

}
