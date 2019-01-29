using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
    public Scrollbar scrollBar;

    public float jumpValue = 0f;
    public float meterIncrement;

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateJumpMeter();
    }

    private void UpdateJumpMeter()
    {
        bool spaceBar = Input.GetKey(KeyCode.Space);

        if(spaceBar)
        {
            jumpValue += meterIncrement * Time.deltaTime;
            scrollBar.value = jumpValue;

            if(jumpValue >= 1)
            {
                // Initiate jump automatically
            }
        }
    }
}
