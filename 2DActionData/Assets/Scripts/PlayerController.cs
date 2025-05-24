using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("�n�ʔ���")]
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

        // ���E���]�i�X�P�[���ێ��j
        if (moveX != 0)
        {
            transform.localScale = new Vector3(originalScale.x * Mathf.Sign(moveX), originalScale.y, originalScale.z);
        }

        // �n�ʃ`�F�b�N
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // �A�j���[�V�����ݒ�
        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("IsJumping", !isGrounded);

        // �W�����v����
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

    // �f�o�b�O�p�ɒn�ʃ`�F�b�N�̉~��Scene�r���[�ɕ\��
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
