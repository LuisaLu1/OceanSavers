

using UnityEngine;
using OscCore;

public class GravityInput : MonoBehaviour
{
    public OscReceiver receiver;

    [Header("OSC Settings")]
    public string address = "/zigsim/gravity";

    [Header("Gravity Data (Read Only)")]
    public Vector3 gravity;

    // thread-safe buffer
    Vector3 gravityBuffer;

    void Start()
    {
        if (receiver == null)
            receiver = GetComponent<OscReceiver>();

        if (receiver != null)
        {
            receiver.Server.TryAddMethod(address, ReadValues);
        }
        else
        {
            Debug.LogError("No OscReceiver found on this GameObject!");
        }
    }

    // Runs on background thread (OSC)
    void ReadValues(OscMessageValues values)
    {
        gravityBuffer.x = values.ReadFloatElement(0);
        gravityBuffer.y = values.ReadFloatElement(1);
        gravityBuffer.z = values.ReadFloatElement(2);
    }

    // Runs on Unity main thread
    void Update()
    {
        gravity = gravityBuffer;
    }
}