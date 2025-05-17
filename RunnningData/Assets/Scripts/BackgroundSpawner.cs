using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    public GameObject backgroundPrefab;
    public float spawnInterval = 2f;
    public float backgroundLifetime = 10f;
    public Vector3 spawnPosition = new Vector3(0, 0, 0);

    void Start()
    {
        InvokeRepeating(nameof(SpawnBackground), 0f, spawnInterval);
    }

    void SpawnBackground()
    {
        GameObject bg = Instantiate(backgroundPrefab, spawnPosition, Quaternion.identity);
        Destroy(bg, backgroundLifetime);
    }
}
