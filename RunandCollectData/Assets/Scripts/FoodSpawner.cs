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
        if (prefab == null || spawnCanvas == null) return;

        // Canvas 内でランダム位置を計算
        float x = Random.Range(0f, spawnCanvas.rect.width);
        float y = Random.Range(0f, spawnCanvas.rect.height);
        Vector2 spawnPos = new Vector2(x, y);

        // FoodをCanvasの子として生成
        GameObject obj = Instantiate(prefab, spawnCanvas);
        obj.GetComponent<RectTransform>().anchoredPosition = spawnPos;
    }
}
