using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;

    public GameObject jumpMeterUI;

    PlayerMovement playerMovement;

    [Header("Jump Configuration")]
    [Tooltip("This value controls the how fast the jump meter increments.")]
    [SerializeField] private float scrollBarValueIncrement; // The amount that the scrollbar value increments by.
    [HideInInspector] public float scrollBarValue; // The amount that the scrollbar is at when we release the spacebar.

    public bool cancelledJump = false;
    public bool isCalculatingJump = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        scrollBar.value = 0f;
    }

    public IEnumerator CalculateJumpForce()
    {
        while (Input.GetButton("Jump")) // Run until we release the space bar.
        {
            if (cancelledJump) yield break;

            if (scrollBarValue >= 1f) // Since the scrollbar value only goes from 0-1, we want to make sure we dont go over 1.
            {
                RestartJump();
                yield break;
            }

            scrollBarValue += scrollBarValueIncrement * Time.deltaTime; // Increment jump force amount by the multiplier we set.
            scrollBar.value = scrollBarValue; // Set the scrollbar value to the jump force amount each deltaTime second.
            yield return new WaitForSeconds(Time.deltaTime);
        }

        playerMovement.CalculateJump(scrollBarValue);
        ResetValues();
        yield break;
    }



    public void ResetValues()
    {
        //isCalculatingJump = false;
        scrollBarValue = 0f;
        scrollBar.value = 0f;
    }

    private void RestartJump()
    {
        ResetValues();
        StartCoroutine(CalculateJumpForce());
    }

    public void StartCalulatingJump()
    {
        cancelledJump = false;
        isCalculatingJump = true;
        jumpMeterUI.SetActive(true);
        StartCoroutine(CalculateJumpForce());
    }

    public void StopCalculatingJump()
    {
        cancelledJump = true;
        isCalculatingJump = false;
        jumpMeterUI.SetActive(false);
        ResetValues();
    }
}
