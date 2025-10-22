//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class PlayerMovementAdvanced : MonoBehaviour
//{
//    [Header("UI References")]
//    public GameObject playerHUD;
//    public GameObject victoryScreen;
//    public GameObject gameOverScreen;

//    [Header("Movement")]
//    private float moveSpeed;
//    private float desiredMoveSpeed;
//    private float lastDesiredMoveSpeed;
//    public float walkSpeed;
//    public float sprintSpeed;
//    public float slideSpeed;
//    public float wallrunSpeed;
//    public float climbSpeed;
//    public float vaultSpeed;
//    public float airMinSpeed;

//    public float speedIncreaseMultiplier;
//    public float slopeIncreaseMultiplier;

//    public float groundDrag;

//    [Header("Jumping")]
//    public float jumpForce;
//    public float jumpCooldown;
//    public float airMultiplier;
//    bool readyToJump;

//    [Header("Crouching")]
//    public float crouchSpeed;
//    public float crouchYScale;
//    private float startYScale;

//    [Header("Keybinds")]
//    public KeyCode jumpKey = KeyCode.Space;
//    public KeyCode sprintKey = KeyCode.LeftShift;
//    public KeyCode crouchKey = KeyCode.LeftControl;

//    [Header("Ground Check")]
//    public float playerHeight;
//    public LayerMask whatIsGround;
//    public bool grounded;

//    [Header("Slope Handling")]
//    public float maxSlopeAngle;
//    private RaycastHit slopeHit;
//    private bool exitingSlope;

//    [Header("References")]
//    public Climbing climbingScript;
//    private ClimbingDone climbingScriptDone;

//    public Transform orientation;

//    float horizontalInput;
//    float verticalInput;

//    Vector3 moveDirection;

//    Rigidbody rb;

//    public MovementState state;
//    public enum MovementState
//    {
//        freeze,
//        unlimited,
//        walking,
//        sprinting,
//        wallrunning,
//        climbing,
//        vaulting,
//        crouching,
//        sliding,
//        air
//    }

//    public bool sliding;
//    public bool crouching;
//    public bool wallrunning;
//    public bool climbing;
//    public bool vaulting;

//    public bool freeze;
//    public bool unlimited;

//    public bool restricted;

//    public TextMeshProUGUI text_speed;
//    public TextMeshProUGUI text_mode;

//    private void Start()
//    {
//        //if (playerHUD != null)
//        //    playerHUD.SetActive(false);

//        if (victoryScreen != null)
//            victoryScreen.SetActive(false);

//        if (gameOverScreen != null)
//            gameOverScreen.SetActive(false);

//        climbingScriptDone = GetComponent<ClimbingDone>();
//        rb = GetComponent<Rigidbody>();
//        rb.freezeRotation = true;

//        readyToJump = true;

//        startYScale = transform.localScale.y;
//    }

//    private void Update()
//    {
//        // ground check
//        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

//        MyInput();
//        SpeedControl();
//        StateHandler();
//        TextStuff();

//        // handle drag
//        if (grounded)
//            rb.linearDamping = groundDrag;
//        else
//            rb.linearDamping = 0;
//    }

//    private void FixedUpdate()
//    {
//        MovePlayer();
//    }

//    private void MyInput()
//    {
//        horizontalInput = Input.GetAxisRaw("Horizontal");
//        verticalInput = Input.GetAxisRaw("Vertical");

//        // when to jump
//        if (Input.GetKey(jumpKey) && readyToJump && grounded)
//        {
//            readyToJump = false;

//            Jump();

//            Invoke(nameof(ResetJump), jumpCooldown);
//        }

//        // start crouch
//        if (Input.GetKeyDown(crouchKey) && horizontalInput == 0 && verticalInput == 0)
//        {
//            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
//            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

//            crouching = true;
//        }

//        // stop crouch
//        if (Input.GetKeyUp(crouchKey))
//        {
//            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

//            crouching = false;
//        }
//    }

//    bool keepMomentum;
//    private void StateHandler()
//    {
//        // Mode - Freeze
//        if (freeze)
//        {
//            state = MovementState.freeze;
//            rb.linearVelocity = Vector3.zero;
//            desiredMoveSpeed = 0f;
//        }

//        // Mode - Unlimited
//        else if (unlimited)
//        {
//            state = MovementState.unlimited;
//            desiredMoveSpeed = 999f;
//        }

//        // Mode - Vaulting
//        else if (vaulting)
//        {
//            state = MovementState.vaulting;
//            desiredMoveSpeed = vaultSpeed;
//        }

//        // Mode - Climbing
//        else if (climbing)
//        {
//            state = MovementState.climbing;
//            desiredMoveSpeed = climbSpeed;
//        }

//        // Mode - Wallrunning
//        else if (wallrunning)
//        {
//            state = MovementState.wallrunning;
//            desiredMoveSpeed = wallrunSpeed;
//        }

//        // Mode - Sliding
//        else if (sliding)
//        {
//            state = MovementState.sliding;

//            // increase speed by one every second
//            if (OnSlope() && rb.linearVelocity.y < 0.1f)
//            {
//                desiredMoveSpeed = slideSpeed;
//                keepMomentum = true;
//            }

//            else
//                desiredMoveSpeed = sprintSpeed;
//        }

//        // Mode - Crouching
//        else if (crouching)
//        {
//            state = MovementState.crouching;
//            desiredMoveSpeed = crouchSpeed;
//        }

//        // Mode - Sprinting
//        else if (grounded && Input.GetKey(sprintKey))
//        {
//            state = MovementState.sprinting;
//            desiredMoveSpeed = sprintSpeed;
//        }

//        // Mode - Walking
//        else if (grounded)
//        {
//            state = MovementState.walking;
//            desiredMoveSpeed = walkSpeed;
//        }

//        // Mode - Air
//        else
//        {
//            state = MovementState.air;

//            if (moveSpeed < airMinSpeed)
//                desiredMoveSpeed = airMinSpeed;
//        }

//        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

//        if (desiredMoveSpeedHasChanged)
//        {
//            if (keepMomentum)
//            {
//                StopAllCoroutines();
//                StartCoroutine(SmoothlyLerpMoveSpeed());
//            }
//            else
//            {
//                moveSpeed = desiredMoveSpeed;
//            }
//        }

//        lastDesiredMoveSpeed = desiredMoveSpeed;

//        // deactivate keepMomentum
//        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
//    }

//    private IEnumerator SmoothlyLerpMoveSpeed()
//    {
//        // smoothly lerp movementSpeed to desired value
//        float time = 0;
//        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
//        float startValue = moveSpeed;

//        while (time < difference)
//        {
//            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

//            if (OnSlope())
//            {
//                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
//                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

//                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
//            }
//            else
//                time += Time.deltaTime * speedIncreaseMultiplier;

//            yield return null;
//        }

//        moveSpeed = desiredMoveSpeed;
//    }

//    private void MovePlayer()
//    {
//        if (climbingScript.exitingWall) return;
//        if (climbingScriptDone.exitingWall) return;
//        if (restricted) return;

//        // calculate movement direction
//        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

//        // on slope
//        if (OnSlope() && !exitingSlope)
//        {
//            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

//            if (rb.linearVelocity.y > 0)
//                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
//        }

//        // on ground
//        else if (grounded)
//            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

//        // in air
//        else if (!grounded)
//            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

//        // turn gravity off while on slope
//        if(!wallrunning) rb.useGravity = !OnSlope();
//    }

//    private void SpeedControl()
//    {
//        // limiting speed on slope
//        if (OnSlope() && !exitingSlope)
//        {
//            if (rb.linearVelocity.magnitude > moveSpeed)
//                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
//        }

//        // limiting speed on ground or in air
//        else
//        {
//            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

//            // limit velocity if needed
//            if (flatVel.magnitude > moveSpeed)
//            {
//                Vector3 limitedVel = flatVel.normalized * moveSpeed;
//                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
//            }
//        }
//    }

//    private void Jump()
//    {
//        exitingSlope = true;

//        // reset y velocity
//        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

//        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
//    }
//    private void ResetJump()
//    {
//        readyToJump = true;

//        exitingSlope = false;
//    }

//    public bool OnSlope()
//    {
//        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
//        {
//            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
//            return angle < maxSlopeAngle && angle != 0;
//        }

//        return false;
//    }

//    public Vector3 GetSlopeMoveDirection(Vector3 direction)
//    {
//        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
//    }

//    private void TextStuff()
//    {
//        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

//        if (OnSlope())
//            text_speed.SetText("Speed: " + Round(rb.linearVelocity.magnitude, 1) + " / " + Round(moveSpeed, 1));

//        else
//            text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(moveSpeed, 1));

//        text_mode.SetText(state.ToString());
//    }

//    public static float Round(float value, int digits)
//    {
//        float mult = Mathf.Pow(10.0f, (float)digits);
//        return Mathf.Round(value * mult) / mult;
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("UI References")]
    public GameObject playerHUD;
    public GameObject victoryScreen;
    public GameObject gameOverScreen;

    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallrunSpeed;
    public float climbSpeed;
    public float vaultSpeed;
    public float airMinSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Dashing")]
    public float dashForce = 25f;
    public float dashUpwardForce = 2f;
    public float maxDashYSpeed = 15f;
    public float dashDuration = 0.2f;
    public KeyCode dashKey = KeyCode.E;
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;
    public float dashCd = 1.0f;
    private float dashCdTimer;
    public bool dashing;
    public float dashFov = 105f;
    public float normalFov = 85f;
    public float maxYSpeed;
    private Vector3 delayedForceToApply;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("References")]
    public Climbing climbingScript;
    private ClimbingDone climbingScriptDone;
    public Transform orientation;
    public PlayerCam cam;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        freeze,
        unlimited,
        walking,
        sprinting,
        wallrunning,
        climbing,
        vaulting,
        crouching,
        sliding,
        dashing,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool wallrunning;
    public bool climbing;
    public bool vaulting;
    public bool freeze;
    public bool unlimited;
    public bool restricted;

    public TextMeshProUGUI text_speed;
    public TextMeshProUGUI text_mode;

    private void Start()
    {
        if (victoryScreen != null)
            victoryScreen.SetActive(false);
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        dashing = false;
        maxYSpeed = 0f;

        climbingScriptDone = GetComponent<ClimbingDone>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        TextStuff();

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        if (Input.GetKeyDown(dashKey))
            Dash();

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKey) && horizontalInput == 0 && verticalInput == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            crouching = true;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            crouching = false;
        }
    }

    bool keepMomentum;
    private void StateHandler()
    {
        if (freeze)
        {
            state = MovementState.freeze;
            rb.linearVelocity = Vector3.zero;
            desiredMoveSpeed = 0f;
        }
        else if (unlimited)
        {
            state = MovementState.unlimited;
            desiredMoveSpeed = 999f;
        }
        else if (vaulting)
        {
            state = MovementState.vaulting;
            desiredMoveSpeed = vaultSpeed;
        }
        else if (climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }
        else if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }
        else if (sliding)
        {
            state = MovementState.sliding;
            if (OnSlope() && rb.linearVelocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
                keepMomentum = true;
            }
            else
                desiredMoveSpeed = sprintSpeed;
        }
        else if (crouching)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashForce; // Here, works for state handler logic
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
            if (moveSpeed < airMinSpeed)
                desiredMoveSpeed = airMinSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (dashing) return;
        if (climbingScript.exitingWall) return;
        if (climbingScriptDone.exitingWall) return;
        if (restricted) return;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        if (!wallrunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        dashCdTimer = dashCd;

        dashing = true;
        maxYSpeed = maxDashYSpeed;
        if (cam != null) cam.DoFov(dashFov);

        Transform forwardT;
        if (useCameraForward && Camera.main != null)
            forwardT = Camera.main.transform;
        else
            forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedDashForce()
    {
        if (resetVel)
            rb.linearVelocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        dashing = false;
        maxYSpeed = 0;
        rb.useGravity = true;
        if (cam != null) cam.DoFov(normalFov);
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3();
        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;
        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;
        return direction.normalized;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void TextStuff()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (OnSlope())
            text_speed.SetText("Speed: " + Round(rb.linearVelocity.magnitude, 1) + " / " + Round(moveSpeed, 1));
        else
            text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(moveSpeed, 1));

        text_mode.SetText(state.ToString());
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
