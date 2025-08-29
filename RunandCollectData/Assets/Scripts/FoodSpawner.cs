using UnityEngine;

[System.Serializable]
public class FoodSpawnData
{
    public GameObject prefab;         // Foodプレハブ（UI用）
    [Range(0f, 1f)] public float spawnChance = 1f;  // スポーン確率
    public float spawnInterval = 3f;  // スポーン間隔（秒）
    [HideInInspector] public float timer;  // 内部用
}

public class FoodSpawner : MonoBehaviour
{
    [Header("スポーンするCanvas")]
    public RectTransform spawnCanvas;  // Foodを置くCanvas

    [Header("プレイヤーUI")]
    public RectTransform playerUI;     // プレイヤーのUI座標

    [Header("スポーン範囲設定")]
    public Vector2 spawnRange = new Vector2(100f, 100f);
    // XとY方向の範囲（例: 100なら±100の範囲）

    [Header("スポーン設定")]
    public FoodSpawnData[] foods;

    void Update()
    {
        foreach (var food in foods)
        {
            food.timer += Time.deltaTime;

            if (food.timer >= food.spawnInterval)
            {
                food.timer = 0f;

                if (Random.value <= food.spawnChance)
                {
                    SpawnFood(food.prefab);
                }
            }
        }
    }

    void SpawnFood(GameObject prefab)
    {
        if (prefab == null || spawnCanvas == null || playerUI == null) return;

        // プレイヤー座標を中心にランダム位置を算出
        Vector2 playerPos = playerUI.anchoredPosition;
        float x = Random.Range(-spawnRange.x, spawnRange.x);
        float y = Random.Range(-spawnRange.y, spawnRange.y);
        Vector2 spawnPos = playerPos + new Vector2(x, y);

        // Canvas内に収まるようにClamp
        float halfWidth = spawnCanvas.rect.width / 2f;
        float halfHeight = spawnCanvas.rect.height / 2f;
        spawnPos.x = Mathf.Clamp(spawnPos.x, -halfWidth, halfWidth);
        spawnPos.y = Mathf.Clamp(spawnPos.y, -halfHeight, halfHeight);

        // FoodをCanvasの子として生成
        GameObject obj = Instantiate(prefab, spawnCanvas);
        obj.GetComponent<RectTransform>().anchoredPosition = spawnPos;
    }
}
