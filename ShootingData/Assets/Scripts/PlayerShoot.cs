using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("�ˏo����v���n�u")]
    public GameObject projectilePrefab;

    [Header("�ˏo�̑��x")]
    public float projectileSpeed = 10f;

    [Header("�X�v���C�g�̎����i�b�j")]
    public float projectileLifetime = 3f;

    [Header("�ˏo�ʒu�̃I�t�Z�b�g")]
    public Vector2 shootOffset = Vector2.right;

    [Header("�����ڂ̉�]�p�x�iZ���E�x�j")]
    public float visualRotationAngle = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���N���b�N�i���N���b�N�j
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
            rb.bodyType = RigidbodyType2D.Dynamic; // �� �O�̂��ߓ��I�ɐݒ�
            rb.velocity = Vector2.right * projectileSpeed;
        }

        Destroy(projectile, projectileLifetime);
    }
}
