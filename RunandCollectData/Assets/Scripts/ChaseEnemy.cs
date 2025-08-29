using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2f;   // 移動速度

    [Header("攻撃設定")]
    public int damage = 10;        // プレイヤーへのダメージ
    public int scorePenalty = 5;   // スコア減少量

    private Transform player;      // プレイヤーのTransform
    private Player playerScript;   // プレイヤースクリプト参照

    void Start()
    {
        // プレイヤーを探す（タグを利用）
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerScript = playerObj.GetComponent<Player>();
        }
    }

    void Update()
    {
        if (player == null) return;

        // プレイヤー方向に移動
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーに当たったら処理
        if (other.CompareTag("Player"))
        {
            if (playerScript != null)
            {
                // ダメージ
                playerScript.currentHP -= damage;
                playerScript.currentHP = Mathf.Clamp(playerScript.currentHP, 0, playerScript.maxHP);

                // スコア減少
                playerScript.AddScore(-scorePenalty);

                Debug.Log("敵に当たった！HP: " + playerScript.currentHP + " スコア: " + playerScript.score);
            }

            // 自分を消す
            Destroy(gameObject);
        }
    }
}
