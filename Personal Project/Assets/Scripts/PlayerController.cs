using UnityEngine;

public class ShipScrollerController : MonoBehaviour
{
    [Header("Forward Speed")]
    public float minSpeed = 3f;       // always drifting forward
    public float maxSpeed = 12f;
    public float accel = 8f;          // W increases speed
    public float decel = 6f;          // S decreases speed
    private float currentSpeed;

    [Header("Lateral Movement")]
    public float lateralSpeed = 8f;   // left/right slide speed
    public float lateralSmoothing = 12f; // how snappy the slide feels
    public float xClamp = 7.5f;       // optional: limit horizontal range

    [Header("Visual Bank (tilt only)")]
    public Transform model;           // assign the ship mesh child
    public float bankAngle = 36f;     // visual tilt at full steer
    public float bankLerp = 10f;

    // Internal
    private float targetX;            // desired world X
    private float currentX;           // smoothed world X

    void Start()
    {
        currentSpeed = Mathf.Max(minSpeed, currentSpeed);
        // Ensure the ship faces world-forward (Z+)
        transform.rotation = Quaternion.identity;
        targetX = transform.position.x;
        currentX = targetX;
    }

    void Update()
    {
        // ---- Input ----
        float steer = Input.GetAxis("Horizontal"); // -1..1 (A/D or ←/→)
        float throttle = Input.GetAxis("Vertical"); // -1..1 (S..W)

        // ---- Speed: persistent, no reverse ----
        if (throttle > 0f)
            currentSpeed += accel * throttle * Time.deltaTime;
        else if (throttle < 0f)
            currentSpeed += decel * throttle * Time.deltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // ---- Lateral movement ----
        targetX += steer * lateralSpeed * Time.deltaTime;
        if (xClamp > 0f)
            targetX = Mathf.Clamp(targetX, -xClamp, xClamp);

        // Smooth X for nicer motion
        currentX = Mathf.Lerp(currentX, targetX, 1f - Mathf.Exp(-lateralSmoothing * Time.deltaTime));

        // Keep forward direction fixed (world Z+); move forward + slide X
        Vector3 pos = transform.position;
        pos.x = currentX;
        pos += Vector3.forward * currentSpeed * Time.deltaTime;
        transform.position = pos;

        // Force heading to face world-forward (no rotation drift)
        transform.rotation = Quaternion.identity;

        // ---- Visual bank (tilt the model only) ----
        if (model != null)
        {
            float targetBank = -steer * bankAngle; // Z-axis tilt
            float targetYaw = steer * bankAngle * 0.5f; // Y-axis turn amount

            Quaternion targetRot = Quaternion.Euler(0f, targetYaw, targetBank);
            model.localRotation = Quaternion.Slerp(
                model.localRotation,
                targetRot,
                bankLerp * Time.deltaTime
            );
        }

    }
}
