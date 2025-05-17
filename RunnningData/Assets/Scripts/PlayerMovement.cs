using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // ← 回転をロックする
    }

    void Update()
    {
        // 常に右に進む
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        // 地面チェック
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // スペースでジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
