using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [Header("吹っ飛ぶ強さ")]
    public float hitForce = 10f; // 力の大きさ

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // コライダーに当たったとき
    private void OnCollisionEnter(Collision collision)
    {
        // 当たった相手がGolfClabタグだったら
        if (collision.gameObject.CompareTag("GolfClab"))
        {
            Debug.Log("Hit");
            // 接触面の法線方向に力を加える
            Vector3 direction = collision.contacts[0].normal * -1;
            rb.AddForce(direction * hitForce, ForceMode.Impulse);
        }
    }
}
