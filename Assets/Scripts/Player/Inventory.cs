using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    int currentSnowballs = 99; // Just for debug
    int currentCoins;

    bool hasShield = false;
    bool hasKey = false;

    

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

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
