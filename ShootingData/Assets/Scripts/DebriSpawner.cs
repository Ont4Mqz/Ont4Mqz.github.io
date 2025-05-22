using UnityEngine;

public class DebriSpawner : MonoBehaviour
{
    [Header("�����X�v���C�g�v���n�u�i�����j")]
    public GameObject[] debrisPrefabs;

    [Header("�����Ԋu�i�b�j")]
    public float spawnInterval = 1f;

    [Header("�ړ����x�i�������j")]
    public float moveSpeed = 3f;

    [Header("�����i�b�j")]
    public float lifetime = 3f;

    [Header("�X�|�[���ʒu�i��ʉE�[��Y�͈́j")]
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

        // �����_���ȃv���n�u��I��
        GameObject prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];

        // �X�|�[���ʒu�i��ʉE����AY���̓����_���j
        Vector3 spawnPosition = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right).x + 1f, Random.Range(minY, maxY), 0);

        // �C���X�^���X��
        GameObject debris = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Rigidbody2D�ō��Ɉړ�
        Rigidbody2D rb = debris.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // �O�͂Ȃ��œ�����
            rb.velocity = Vector2.left * moveSpeed;
        }

        // �����ō폜
        Destroy(debris, lifetime);
    }
}
