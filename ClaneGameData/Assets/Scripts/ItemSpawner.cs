using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float spawnInterval = 0.2f;
    [SerializeField] private int initialSpawnCount = 5;

    public int InitialSpawnCount => initialSpawnCount;

    void Start()
    {
        SpawnRandom(initialSpawnCount);
    }

    // --------------------
    // 指定プレハブを指定数スポーン
    // ★ 今回エラーの原因だったやつ
    // --------------------
    public void Spawn(GameObject prefab, int count)
    {
        StartCoroutine(SpawnRoutine(prefab, count));
    }

    IEnumerator SpawnRoutine(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform point =
                spawnPoints[Random.Range(0, spawnPoints.Count)];

            Instantiate(prefab, point.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // --------------------
    // 完全ランダムスポーン
    // --------------------
    public void SpawnRandom(int count)
    {
        StartCoroutine(SpawnRandomRoutine(count));
    }

    IEnumerator SpawnRandomRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab =
                itemPrefabs[Random.Range(0, itemPrefabs.Count)];

            Transform point =
                spawnPoints[Random.Range(0, spawnPoints.Count)];

            Instantiate(prefab, point.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
