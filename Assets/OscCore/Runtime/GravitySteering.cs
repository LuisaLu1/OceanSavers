


using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySteering : MonoBehaviour
{
    [Header("Steering Settings")]
    public float turnStrength = 5f;
    public float smoothing = 5f;

    private Rigidbody rb;
    private float smoothX;

    private Vector3 gravityValue;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Smooth gravity X (phone tilt left/right)
        smoothX = Mathf.Lerp(
            smoothX,
            //gravityInput.gravity.x,
            gravityValue.y,
            Time.fixedDeltaTime * smoothing
        );

        //Debug.Log("smoothX: " + smoothX);

        // Apply steering torque (Y axis rotation)
        rb.AddTorque(Vector3.up * smoothX * turnStrength, ForceMode.Force);
    }

    public void GetGravity(Vector3 gravity)
    {
        //Debug.Log("Gravity OSC: " + gravity);

        gravityValue = gravity;
    }
}