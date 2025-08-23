using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHP = 100; // 最大HP
    public int currentHP;
    public int score = 0;   // スコア

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    public float hpDecayRate = 1f; // 1秒ごとに減るHP量
    private float decayTimer;

    [SerializeField] private GameObject GameOverPanel;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHP = maxHP; // スタート時は満タン
    }

    void Update()
    {
        // WASD入力
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // アニメーション
        animator.SetBool("isWalking", moveInput != Vector2.zero);
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        // HPが時間で減少
        decayTimer += Time.deltaTime;
        if (decayTimer >= 1f)
        {
            currentHP -= Mathf.RoundToInt(hpDecayRate);
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            decayTimer = 0f;

            if (currentHP <= 0)
            {
                gameObject.SetActive(GameOverPanel);
                // ここでシーン遷移や死亡処理を呼ぶ
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    // === Food から呼ばれる関数 ===
    public void AddHP(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        Debug.Log("HP回復: " + currentHP);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("スコア: " + score);
    }
}
