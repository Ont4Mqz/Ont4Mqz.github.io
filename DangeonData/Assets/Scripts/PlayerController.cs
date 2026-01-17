using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f; // 移動速度

    private Rigidbody2D rb;       
    private Vector2 input;        

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D取得
    }

    private void Update()
    {
        // 入力取得
        input = new Vector2(
            Input.GetAxisRaw("Horizontal"),   // A,D 左右
            Input.GetAxisRaw("Vertical")      // W,S 上下
        ).normalized;
    }

    private void FixedUpdate()
    {
        // 物理移動
        rb.velocity = input * moveSpeed;
    }
}
