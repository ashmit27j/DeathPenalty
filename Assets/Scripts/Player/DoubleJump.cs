using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleJump : MonoBehaviour
{
    [Header("Double Jump Settings")]
    [SerializeField] private int maxJumps = 2; // 1 on ground + 1 double jump
    private int jumpsRemaining;

    [Header("UI References")]
    [SerializeField] private Image doubleJumpIndicator;
    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color usedColor = Color.gray;
    [SerializeField, Range(0f, 1f)] private float fadeAlphaUsed = 0.2f;
    [SerializeField, Range(0f, 1f)] private float fadeAlphaAvailable = 0.7f; // 70% opacity

    // Cached component references
    private Rigidbody rb;
    private PlayerMovementAdvanced playerMovement;
    private WallRunningAdvanced wallRunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovementAdvanced>();
        wallRunning = GetComponent<WallRunningAdvanced>();

        jumpsRemaining = maxJumps;

        if (doubleJumpIndicator != null)
        {
            doubleJumpIndicator.type = Image.Type.Filled;
            doubleJumpIndicator.fillMethod = Image.FillMethod.Radial360;
            doubleJumpIndicator.fillOrigin = (int)Image.Origin360.Top;
            doubleJumpIndicator.fillClockwise = false;
            UpdateUI();
        }
    }

    private void Update()
    {
        if ((playerMovement.grounded || IsTouchingWall()) && rb.linearVelocity.y <= 0.1f)
        {
            ResetJumps();
        }
    }

    private bool IsTouchingWall()
    {
        if (wallRunning != null)
        {
            return playerMovement.wallrunning;
        }
        return false;
    }

    public bool CanJump()
    {
        return jumpsRemaining > 0;
    }

    public void UseJump()
    {
        if (jumpsRemaining > 0)
        {
            jumpsRemaining--;
            UpdateUI();
        }
    }

    private void ResetJumps()
    {
        if (jumpsRemaining < maxJumps)
        {
            jumpsRemaining = maxJumps;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (doubleJumpIndicator != null)
        {
            // Always show icon fully filled
            doubleJumpIndicator.fillAmount = 1f;

            // Set color and 70% alpha if available, otherwise faded
            Color c = jumpsRemaining > 1 ? availableColor : usedColor;
            c.a = jumpsRemaining > 1 ? fadeAlphaAvailable : fadeAlphaUsed;
            doubleJumpIndicator.color = c;
        }
    }


    public int GetRemainingJumps()
    {
        return jumpsRemaining;
    }
}
