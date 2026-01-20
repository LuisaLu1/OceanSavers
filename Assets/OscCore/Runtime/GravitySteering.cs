

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
    // 1. Move Forward (Z)
    float nextZ = transform.position.z + (forwardSpeed * Time.fixedDeltaTime);
    
    // 2. Determine Horizontal Input
    // CHANGE THIS: Try gravityValue.y first. If that doesn't work, try gravityValue.z
    float horizontalInput = -gravityValue.y; 

    // 3. Calculate Steering (X)
    float nextX = transform.position.x + (horizontalInput * steeringSpeed * Time.fixedDeltaTime);
    nextX = Mathf.Clamp(nextX, -horizontalLimit, horizontalLimit);

    // 4. Apply Position
    // Ensure the Rigidbody 'Is Kinematic' is checked in the Inspector
    rb.MovePosition(new Vector3(nextX, transform.position.y, nextZ));

    // 5. Visual Lean
    if (visualModel != null)
    {
        float leanTarget = horizontalInput * maxLeanAngle;
        visualModel.localRotation = Quaternion.Lerp(visualModel.localRotation, 
            Quaternion.Euler(0, 0, leanTarget), Time.fixedDeltaTime * leanSmoothing);
    }
}

    // This receives the Vector3 from the OscCore Vector3 Input component
    public void GetGravity(Vector3 gravity)
    {
        gravityValue = gravity;
    }
}