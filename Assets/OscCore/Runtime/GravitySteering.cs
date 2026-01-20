

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
        // Using isKinematic prevents the linear velocity error on kinematic bodies
        rb.isKinematic = true; 
    }

    void FixedUpdate()
    {
        // 1. Automatic Forward Movement (Z axis)
        float nextZ = transform.position.z + (forwardSpeed * Time.fixedDeltaTime);
        
        // 2. Steering (X axis) using phone Z-tilt 
        // We use .z here because you mentioned X is not receiving data
        float horizontalInput = -gravityValue.z; 
        float nextX = transform.position.x + (horizontalInput * steeringSpeed * Time.fixedDeltaTime);

        // 3. Keep within the river/track bounds
        nextX = Mathf.Clamp(nextX, -horizontalLimit, horizontalLimit);

        // 4. Move the Kinematic Rigidbody smoothly
        Vector3 newPosition = new Vector3(nextX, transform.position.y, nextZ);
        rb.MovePosition(newPosition);

        // 5. Visual Tilt
        if (visualModel != null)
        {
            float leanTarget = horizontalInput * maxLeanAngle;
            Quaternion targetRotation = Quaternion.Euler(0, 0, leanTarget);
            visualModel.localRotation = Quaternion.Lerp(visualModel.localRotation, targetRotation, Time.fixedDeltaTime * leanSmoothing);
        }
    }

    // This receives the Vector3 from the OscCore Vector3 Input component
    public void GetGravity(Vector3 gravity)
    {
        gravityValue = gravity;
    }
}