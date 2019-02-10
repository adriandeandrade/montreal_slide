using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxAmountOfSnowballs;
    private int currentSnowballs;
    private int currentCoins;
    private bool hasShield = false;
    private bool hasKey = false;

    public bool HasShield
    {
        get
        {
            return hasShield;
        } set
        {
            hasShield = value;
        }
    }

    public bool HasKey
    {
        get
        {
            return hasKey;
        }
        set
        {
            hasKey = value;
        }
    }

    public int CurrentCoins
    {
        get
        {
            return currentCoins;
        }
        set
        {
            currentCoins = value;
        }
    }

    public int CurrentSnowballs
    {
        get
        {
            return currentSnowballs;
        }
        set
        {
            currentSnowballs = value;
        }
    }
}
