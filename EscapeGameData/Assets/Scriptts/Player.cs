using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // “ü—Í‚Ìæ“¾
        float moveX = Input.GetAxisRaw("Horizontal"); // A,D ‚Ü‚½‚Í ©,¨
        float moveY = Input.GetAxisRaw("Vertical");   // W,S ‚Ü‚½‚Í ª,«

        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // ˆÚ“®ˆ—
        rb.velocity = moveInput * moveSpeed;
    }
}
