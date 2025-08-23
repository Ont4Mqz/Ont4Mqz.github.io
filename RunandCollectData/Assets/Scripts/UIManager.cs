using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshProを使う場合

public class UIManager : MonoBehaviour
{
    [Header("UI参照")]
    public TextMeshProUGUI scoreText; // スコア表示用
    public Slider hpSlider;           // HPバー

    private Player player;

    void Start()
    {
        // Playerを探して参照
        player = FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("Playerがシーン内に見つかりません");
        }

        // HPバーの最大値設定
        if (hpSlider != null)
        {
            hpSlider.maxValue = player.maxHP;
            hpSlider.value = player.currentHP;
        }
    }

    void Update()
    {
        if (player == null) return;

        // スコア更新
        if (scoreText != null)
        {
            scoreText.text = "Score: " + player.score;
        }

        // HP更新
        if (hpSlider != null)
        {
            hpSlider.value = player.currentHP;
        }
    }
}
