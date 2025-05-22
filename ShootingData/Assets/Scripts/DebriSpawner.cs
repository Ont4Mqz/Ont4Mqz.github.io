using UnityEngine;

public class DebriSpawner : MonoBehaviour
{
    [Header("流すスプライトプレハブ（複数可）")]
    public GameObject[] debrisPrefabs;

    [Header("生成間隔（秒）")]
    public float spawnInterval = 1f;

    [Header("移動速度（左方向）")]
    public float moveSpeed = 3f;

    [Header("寿命（秒）")]
    public float lifetime = 3f;

    [Header("スポーン位置（画面右端のY範囲）")]
    public float minY = -3f;
    public float maxY = 3f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnDebris();
            timer = 0f;
        }
    }

    void SpawnDebris()
    {
        if (debrisPrefabs.Length == 0) return;

        // ランダムなプレハブを選ぶ
        GameObject prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];

        // スポーン位置（画面右から、Y軸はランダム）
        Vector3 spawnPosition = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right).x + 1f, Random.Range(minY, maxY), 0);

        // インスタンス化
        GameObject debris = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Rigidbody2Dで左に移動
        Rigidbody2D rb = debris.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // 外力なしで動かす
            rb.velocity = Vector2.left * moveSpeed;
        }

        // 寿命で削除
        Destroy(debris, lifetime);
    }
}
