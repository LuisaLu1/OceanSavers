using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1.0f;

    public Vector3 angles = new Vector3(15, 30, 45);

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime * angles);
    }
}
