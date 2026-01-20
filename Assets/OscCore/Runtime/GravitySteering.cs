

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySteering : MonoBehaviour
{
    [UnityEngine.Header("Movement Settings")]
    public float forwardSpeed = 8f;      
    public float steeringSpeed = 15f;    
    public float horizontalLimit = 12f;  

    [UnityEngine.Header("Visual Lean")]
    public Transform visualModel; 
    public float maxLeanAngle = 25f;
    public float leanSmoothing = 10f;

    private Rigidbody rb;
    private Vector3 gravityValue;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Must be kinematic to move manually via MovePosition
        rb.isKinematic = true; 
    }

    void FixedUpdate()
    {
        // 1. Calculate the New Position
        // Forward is Z, Side-to-Side is X
        float nextZ = transform.position.z + (forwardSpeed * Time.fixedDeltaTime);
        
        // Use Zig Sim Y-tilt for X steering
        float horizontalInput = -gravityValue.y; 
        float nextX = transform.position.x + (horizontalInput * steeringSpeed * Time.fixedDeltaTime);

        // 2. Clamp the X so you stay in the water
        nextX = Mathf.Clamp(nextX, -horizontalLimit, horizontalLimit);

        // 3. Apply the movement to the Rigidbody
        // We keep the current Y position so it doesn't sink
        Vector3 newPosition = new Vector3(nextX, transform.position.y, nextZ);
        rb.MovePosition(newPosition);

        // 4. Visual Tilt (Leaning the child model)
        if (visualModel != null)
        {
            float leanTarget = horizontalInput * maxLeanAngle;
            Quaternion targetRotation = Quaternion.Euler(0, 0, leanTarget);
            visualModel.localRotation = Quaternion.Lerp(visualModel.localRotation, targetRotation, Time.fixedDeltaTime * leanSmoothing);
        }
    }

    // This receives the data from the OscCore Vector3 Input component
    public void GetGravity(Vector3 gravity)
    {
        gravityValue = gravity;
    }
}