using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashingUIHandler : MonoBehaviour
{
    [Header("Dash UI References")]
    [SerializeField] private Image dashIndicator;    // Assign your dash icon image here
    [SerializeField] private Color readyColor = Color.white;
    [SerializeField] private Color cooldownColor = Color.gray;
    [SerializeField, Range(0f, 1f)] private float alphaReady = 1f;     // Show as 70% opacity when ready
    [SerializeField, Range(0f, 1f)] private float alphaCooldown = 0.2f;  // Transparent when on cooldown

    [Header("Dash Logic References")]
    [SerializeField] private PlayerMovementAdvanced playerMovement;      // Hook up your player movement script
    [SerializeField] private float dashCooldown = 1.0f;                  // Must match PlayerMovementAdvanced.dashCd

    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    private void Start()
    {
        if (dashIndicator != null)
        {
            dashIndicator.type = Image.Type.Filled;
            dashIndicator.fillMethod = Image.FillMethod.Radial360;
            dashIndicator.fillOrigin = (int)Image.Origin360.Top;
            dashIndicator.fillClockwise = false;
            dashIndicator.fillAmount = 1f;

            SetReadyUI();
        }
    }

    private void Update()
    {
        // Detect dash input and cooldown
        if (playerMovement != null)
        {
            // Dash just started if dashing flag is true and not on cooldown
            if (playerMovement.dashing && !isOnCooldown)
            {
                StartCooldown();
            }
        }

        // Progress cooldown if active
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(cooldownTimer / dashCooldown);

            if (dashIndicator != null)
            {
                dashIndicator.fillAmount = progress;
            }

            if (cooldownTimer >= dashCooldown)
            {
                EndCooldown();
            }
        }
    }

    private void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = 0f;

        if (dashIndicator != null)
        {
            dashIndicator.fillAmount = 0f;
            Color c = cooldownColor;
            c.a = alphaCooldown;
            dashIndicator.color = c;
        }
    }

    private void EndCooldown()
    {
        isOnCooldown = false;
        cooldownTimer = 0f;
        SetReadyUI();
    }

    private void SetReadyUI()
    {
        if (dashIndicator != null)
        {
            dashIndicator.fillAmount = 1f;
            Color c = readyColor;
            c.a = alphaReady;
            dashIndicator.color = c;
        }
    }
}
