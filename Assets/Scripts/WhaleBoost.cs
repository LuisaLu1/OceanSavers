using UnityEngine;

public class WhaleBoost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.name + " tag:" + other.tag);

        var pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            Debug.Log("BOOST APPLIED");
            pc.ApplyWhaleBoost(); // ← parantez ÇOK ÖNEMLİ
        }
    }
}
