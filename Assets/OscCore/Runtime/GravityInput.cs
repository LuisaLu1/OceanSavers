


using UnityEngine;
using OscCore;

public class GravityInput : MonoBehaviour
{
    public OscReceiver receiver;

    [Header("OSC Settings")]
    public string address = "/alex/gravity";

    [Header("Gravity Data")]
    public Vector3 gravity;

    // Buffer for background thread
    Vector3 gravityBuffer;

    void Start()
    {
        // Find OscReceiver on the same GameObject
        if (receiver == null)
            receiver = GetComponent<OscReceiver>();

        if (receiver == null)
        {
            Debug.LogError("OscReceiver component missing!");
            return;
        }

        // Register OSC address
        receiver.Server.TryAddMethod(address, ReadValues);
    }

    // Runs on OSC background thread
    void ReadValues(OscMessageValues values)
    {
	Debug.Log("Gravity OSC: " + gravityBuffer);
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

