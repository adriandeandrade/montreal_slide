using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { SHIELD, COIN, KEY, SNOWBALL };
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
        switch(itemType)
        {
            case ItemType.SHIELD:
                InitShield();
                break;
            case ItemType.COIN:
                InitCoin();
                break;
            case ItemType.KEY:
                InitKey();
                break;
            case ItemType.SNOWBALL:
                InitSnowball();
                break;
        }
    }

    private void InitShield()
    {
        if(!inventory.HasShield)
        {
            inventory.HasShield = true;
            player.OnGetShield.Invoke();
        }
    }

    private void InitCoin()
    {

    }

    private void InitKey()
    {

    }

    private void InitSnowball()
    {
        if(player.CurrentSnowballs < player.maxSnowballs)
        {
            player.CurrentSnowballs += 1;
        }
    }
}
