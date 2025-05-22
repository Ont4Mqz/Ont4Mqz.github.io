using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("射出するプレハブ")]
    public GameObject projectilePrefab;

    [Header("射出の速度")]
    public float projectileSpeed = 10f;

    [Header("スプライトの寿命（秒）")]
    public float projectileLifetime = 3f;

    [Header("射出位置のオフセット")]
    public Vector2 shootOffset = Vector2.right;

    [Header("見た目の回転角度（Z軸・度）")]
    public float visualRotationAngle = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ←クリック（左クリック）
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position + (Vector3)shootOffset;

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.Euler(0, 0, visualRotationAngle)
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // ← 念のため動的に設定
            rb.velocity = Vector2.right * projectileSpeed;
        }

        Destroy(projectile, projectileLifetime);
    }
}
