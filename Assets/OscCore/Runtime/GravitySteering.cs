


using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySteering : MonoBehaviour
{
    [Header("References")]
    public GravityInput gravityInput;

    [Header("Steering Settings")]
    public float turnStrength = 5f;
    public float smoothing = 5f;

    private Rigidbody rb;
    private float smoothX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (gravityInput == null)
            gravityInput = FindObjectOfType<GravityInput>();

        if (gravityInput == null)
            Debug.LogError("GravityInput not found in scene!");
    }

    void FixedUpdate()
    {
        if (gravityInput == null) return;

        // Smooth gravity X (phone tilt left/right)
        smoothX = Mathf.Lerp(
            smoothX,
            gravityInput.gravity.x,
            Time.fixedDeltaTime * smoothing
        );

        // Apply steering torque (Y axis rotation)
        rb.AddTorque(Vector3.up * smoothX * turnStrength, ForceMode.Force);
    }
}