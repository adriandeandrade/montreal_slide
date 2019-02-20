using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    Timer timer;

    int currentSnowballs = 10; // Just for debug
    int currentCoins;

    bool hasShield = false;
    bool hasBriefcase = true;
    bool hasKey = false;

    

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        timer = FindObjectOfType<Timer>();
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

    public bool HasBriefcase
    {
        get
        {
            return hasBriefcase;
        }
        set
        {
            hasBriefcase = value;
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

    public void AddTime(float time)
    {
        timer.AddTime(time);
    }
}
