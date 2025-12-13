using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs; // スポーンするアイテムのプレハブリスト
    [SerializeField] private List<Transform> spawnPoints; // スポーン位置のリスト
    [SerializeField] private float spawnInterval = 0.2f; // スポーン間隔

    public void Spawn(GameObject prefab, int count) // 指定したプレハブを指定数スポーン
    {
        StartCoroutine(SpawnRoutine(prefab, count));
    }

    IEnumerator SpawnRoutine(GameObject prefab, int count) // 指定したプレハブを指定数スポーンするコルーチン
    {
        for (int i = 0; i < count; i++)
        {
            Transform point =
                spawnPoints[Random.Range(0, spawnPoints.Count)]; // ランダムにスポーン位置を選択

            Instantiate(prefab, point.position, Quaternion.identity); // アイテムをスポーン
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnRandom(List<GameObject> prefabs, int count) // ランダムなプレハブを指定数スポーン
    {
        StartCoroutine(SpawnRandomRoutine(prefabs, count));
    }

    IEnumerator SpawnRandomRoutine(List<GameObject> prefabs, int count) // ランダムなプレハブを指定数スポーンするコルーチン
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab =
                prefabs[Random.Range(0, prefabs.Count)]; // ランダムにプレハブを選択

            Transform point =
                spawnPoints[Random.Range(0, spawnPoints.Count)]; // ランダムにスポーン位置を選択

            Instantiate(prefab, point.position, Quaternion.identity); // アイテムをスポーン
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
