using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 12f, -8f);

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position + offset;
        transform.LookAt(player);
    }
}

