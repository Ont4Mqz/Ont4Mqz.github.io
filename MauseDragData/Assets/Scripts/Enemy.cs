using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("ステータス設定")]
    public float maxHP = 100f;     // 最大HP
    private float currentHP;       // 現在のHP
    public float moveSpeed = 2f;   // 移動速度
    public float damageAmount = 20f; // Swordで受けるダメージ量

    [Header("ターゲット設定")]
    public Transform target;       // 追いかける対象

    [Header("UI参照")]
    public Slider hpSlider;        // HPバー用スライダー

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    void Update()
    {
        MoveTowardTarget();
        UpdateHPBar();
    }

    // ターゲットに向かって移動
    void MoveTowardTarget()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    // ダメージ処理
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    // HPバー更新
    void UpdateHPBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }
    }

    // 死亡処理
    void Die()
    {
        Destroy(gameObject); // 消すだけ
    }

    // 当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Swordタグに当たったらダメージ
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(damageAmount);
        }
    }
}
