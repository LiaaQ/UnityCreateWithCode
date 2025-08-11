using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Forward Speed Settings")]
    public float minSpeed = 3f;          // Minimum forward speed (constant drifting forward)
    public float maxSpeed = 12f;         // Maximum forward speed
    public float accel = 8f;             // Acceleration rate when pressing W / Up
    public float decel = 6f;             // Deceleration rate when pressing S / Down
    private float currentSpeed;

    [Header("Lateral Movement Settings")]
    public float lateralSpeed = 8f;      // Speed for left/right movement
    public float lateralSmoothing = 12f; // Smoothness factor for lateral movement
    public float xClamp = 15.5f;         // Limits horizontal movement boundaries

    [Header("Visual Bank (Ship Tilt)")]
    public Transform model;              // Reference to ship mesh to tilt visually
    public float bankAngle = 36f;        // Maximum tilt angle (degrees) when steering fully
    public float bankLerp = 10f;         // How quickly the tilt interpolates

    // Internal state tracking
    private float targetX;               // Target X position based on input
    private float currentX;              // Smoothed current X position for smooth lateral movement
    private float fixedY;                // Locked Y position to prevent vertical drifting

    private Rigidbody playerRb;          // Rigidbody component reference

    void Start()
    {
        // Cache Rigidbody and verify it exists
        playerRb = GetComponent<Rigidbody>();
        if (playerRb == null)
        {
            Debug.LogError("Rigidbody component missing from ship!");
        }

        // Lock Y position to initial value to prevent vertical drifting
        fixedY = transform.position.y;

        // Initialize current speed to minimum speed or higher if already set
        currentSpeed = Mathf.Max(minSpeed, currentSpeed);

        // Reset rotation to face forward along world Z+ axis
        transform.rotation = Quaternion.identity;

        // Initialize lateral position trackers
        targetX = transform.position.x;
        currentX = targetX;
    }

    void Update()
    {
        // Read player input for steering (left/right) and throttle (forward/back)
        float steer = Input.GetAxis("Horizontal");   // Range: -1 (left) to 1 (right)
        float throttle = Input.GetAxis("Vertical");  // Range: -1 (slow down) to 1 (speed up)

        // Adjust current forward speed based on throttle input, no reverse allowed
        if (throttle > 0f)
            currentSpeed += accel * throttle * Time.deltaTime;
        else if (throttle < 0f)
            currentSpeed += decel * throttle * Time.deltaTime;

        ClampSpeedAndPosition();

        // Update lateral target position based on steering input
        targetX += steer * lateralSpeed * Time.deltaTime;

        // Smooth lateral movement for natural sliding effect
        currentX = Mathf.Lerp(currentX, targetX, 1f - Mathf.Exp(-lateralSmoothing * Time.deltaTime));

        // Update visual model tilt based on steering input
        UpdateVisualModel(steer);
    }

    void FixedUpdate()
    {
        // Calculate lateral velocity needed to reach currentX in fixed timestep
        float lateralVelocity = (currentX - playerRb.position.x) / Time.fixedDeltaTime;

        Vector3 velocity = new Vector3(lateralVelocity, 0f, currentSpeed);
        playerRb.linearVelocity = velocity;

        Vector3 lockedPos = playerRb.position;
        lockedPos.y = fixedY;
        playerRb.position = lockedPos;

        // Reset rotation to identity to prevent physics-induced rotation drift
        playerRb.rotation = Quaternion.identity;
    }

    void UpdateVisualModel(float steer)
    {
        if (model != null)
        {
            // Calculate desired bank (roll) and yaw rotation angles based on steering input
            float targetBank = -steer * bankAngle;          // Roll (Z-axis)
            float targetYaw = steer * bankAngle * 0.5f;     // Yaw (Y-axis)

            Quaternion targetRot = Quaternion.Euler(0f, targetYaw, targetBank);

            if (Mathf.Approximately(steer, 0f))
            {
                // No steering input: smoothly return model to neutral rotation
                model.localRotation = Quaternion.Slerp(model.localRotation, Quaternion.identity, bankLerp * Time.deltaTime);
            }
            else
            {
                // Steering input active: smoothly tilt towards target banking rotation
                model.localRotation = Quaternion.Slerp(model.localRotation, targetRot, bankLerp * Time.deltaTime);
            }
        }
    }

    void ClampSpeedAndPosition()
    {
        // Clamp speed to defined minimum and maximum
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // Clamp lateral position to stay within defined horizontal bounds
        if (xClamp > 0f)
            targetX = Mathf.Clamp(targetX, -xClamp, xClamp);
    }
}
