using UnityEngine;

public class WhaleBoost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var pc = other.GetComponent<PlayerController>();
        if (pc != null) pc.ApplyWhaleBoost();
    }
}

