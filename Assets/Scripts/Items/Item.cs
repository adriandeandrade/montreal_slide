using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { SHIELD, COIN, BRIEFCASE, SNOWBALL };
    public ItemType itemType;

    private Inventory inventory;
    private Player player;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<Player>();
    }

    private void OnDestroy()
    {
        player.isPickingUp = false;
    }

    public void Init()
    {
        switch (itemType)
        {
            case ItemType.SHIELD:
                InitShield();
                break;
            case ItemType.COIN:
                InitCoin();
                break;
            case ItemType.BRIEFCASE:
                InitBriefCase();
                break;
            case ItemType.SNOWBALL:
                InitSnowball();
                break;
        }
    }

    private void InitShield()
    {
        if (!inventory.HasShield)
        {
            inventory.HasShield = true;
            player.OnGetShield.Invoke();
        }
    }

    private void InitCoin()
    {
        inventory.CurrentCoins += 1;
        Debug.Log("Added coin");
        inventory.AddTime(5f);
    }

    private void InitBriefCase()
    {
        inventory.HasBriefcase = true;
    }

    private void InitSnowball()
    {
        inventory.CurrentSnowballs += 1;
    }
}
