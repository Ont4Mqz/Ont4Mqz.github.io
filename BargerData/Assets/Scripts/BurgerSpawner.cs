using UnityEngine;

public class BurgerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // 生成位置

    public void Spawn(GameObject prefab)            // 具材を生成
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
