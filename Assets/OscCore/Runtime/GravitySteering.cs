


using UnityEngine;

public class GravitySteering : MonoBehaviour
{
    private GravityInput gravityInput;
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float smoothing = 6f;

    Vector3 smoothGravity;

    void Start()
    {
        gravityInput = FindObjectOfType<GravityInput>();
        if (gravityInput == null)
        {
            Debug.LogError("GravityInput not found in scene!");
        }

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to this GameObject!");
        }
    }

    void FixedUpdate()
    {
        if (gravityInput == null || rb == null) return;

        // Smooth gravity data
        smoothGravity = Vector3.Lerp(
            smoothGravity,
            gravityInput.gravity,
            Time.fixedDeltaTime * smoothing
        );

        // Apply force
        Vector3 force = new Vector3(
            smoothGravity.x,
            0f,
            smoothGravity.y
        );

        rb.AddForce(force * moveSpeed);
    }
}