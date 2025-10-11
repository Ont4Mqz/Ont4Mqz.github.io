using UnityEngine;
using UnityEngine.UI; // ← Slider用

public class Enemy : MonoBehaviour
{
    [Header("敵の設定")]
    public float maxHP = 100f; // 最大HP
    private float currentHP;

    [Header("Boomerang関連")]
    public float damageFromBoomerang = 20f; // 当たった時のダメージ量

    [Header("HPバーUI")]
    [SerializeField] private Slider hpSlider; // インスペクターで設定

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Boomerangタグのオブジェクトに当たったらダメージ
        if (collision.CompareTag("Boomerang"))
        {
            TakeDamage(damageFromBoomerang);
        }
    }

    void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateHPBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP / maxHP;
        }
    }

    void Die()
    {
        // 死亡時の処理（あとでアニメーションなど追加してもOK）
        Destroy(gameObject);
    }
}
