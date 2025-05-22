using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public Camera mainCamera; // Inspector�ŃJ�������w��i���w��Ȃ玩���擾�j

    void Start()
    {
     
    }

    void Update()
    {
        // ���͎擾
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // �ړ�
        Vector3 movement = new Vector3(moveX, moveY, 0).normalized;
        transform.position += movement * speed * Time.deltaTime;

        // �J�����\���͈͂��擾
        Vector3 camPos = mainCamera.transform.position;
        float vertExtent = mainCamera.orthographicSize;
        float horzExtent = vertExtent * mainCamera.aspect;

        // �v���C���[�̔��a�i�X�v���C�g�T�C�Y�̔����Ȃǁj���l������ꍇ�͂����Œ���
        float minX = camPos.x - horzExtent;
        float maxX = camPos.x + horzExtent;
        float minY = camPos.y - vertExtent;
        float maxY = camPos.y + vertExtent;

        // �J�����͈͓���Clamp
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }
}
