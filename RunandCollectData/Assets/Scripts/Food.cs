using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("このFoodが回復するHP量")]
    public int healAmount = 10;

    [Header("このFoodが与えるスコア")]
    public int scoreValue = 100;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player スクリプトを取得
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddHP(healAmount);     // HP回復
                player.AddScore(scoreValue); // スコア加算
            }

            // 自分自身を消す
            Destroy(gameObject);
        }
    }
}
