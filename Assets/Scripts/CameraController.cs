using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 0f, 0f);


void Start()
{
    // Berechnet Offset beim Spielstart
    offset = transform.position - player.position;
}

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position + offset;
        //transform.LookAt(player);
    }
}

