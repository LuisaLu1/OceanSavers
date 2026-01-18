using UnityEngine;

public class WhaleBoost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.name + " tag:" + other.tag);

        // child collider -> parent'taki PlayerController'ı bul
        var pc = other.GetComponentInParent<PlayerController>();

        if (pc != null)
        {
            Debug.Log("BOOST APPLIED");
            pc.ApplyWhaleBoost();
        }
        else
        {
            Debug.LogWarning("PlayerController bulunamadı! (Is Script on parent ?)");
        }
    }
}
