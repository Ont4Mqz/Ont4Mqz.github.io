using UnityEngine;

public class SpawnInFrontOfPlayer : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 2f;
    public float objectLifetime = 5f;

    public float xOffsetMin = 5f;
    public float xOffsetMax = 10f;
    public float yOffsetMin = -2f;
    public float yOffsetMax = 2f;

    public Transform player; // プレイヤーのTransform
    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval);
    }

    void SpawnObject()
    {
        // プレイヤーの右前方にランダム生成
        float x = Random.Range(xOffsetMin, xOffsetMax);
        float y = Random.Range(yOffsetMin, yOffsetMax);
        Vector3 spawnPos = new Vector3(player.position.x + x, player.position.y + y, 0f);

        GameObject obj = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        Destroy(obj, objectLifetime);
    }
}
