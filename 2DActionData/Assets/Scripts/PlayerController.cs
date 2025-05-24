using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("地面判定")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 originalScale;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        // 左右反転（スケール維持）
        if (moveX != 0)
        {
            transform.localScale = new Vector3(originalScale.x * Mathf.Sign(moveX), originalScale.y, originalScale.z);
        }

        // 地面チェック
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // アニメーション設定
        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("IsJumping", !isGrounded);

        // ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }

    // デバッグ用に地面チェックの円をSceneビューに表示
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
