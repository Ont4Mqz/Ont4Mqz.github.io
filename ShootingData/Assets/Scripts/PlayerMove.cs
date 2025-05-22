using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public Camera mainCamera; // Inspectorでカメラを指定（未指定なら自動取得）

    void Start()
    {
     
    }

    void Update()
    {
        // 入力取得
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 移動
        Vector3 movement = new Vector3(moveX, moveY, 0).normalized;
        transform.position += movement * speed * Time.deltaTime;

        // カメラ表示範囲を取得
        Vector3 camPos = mainCamera.transform.position;
        float vertExtent = mainCamera.orthographicSize;
        float horzExtent = vertExtent * mainCamera.aspect;

        // プレイヤーの半径（スプライトサイズの半分など）を考慮する場合はここで調整
        float minX = camPos.x - horzExtent;
        float maxX = camPos.x + horzExtent;
        float minY = camPos.y - vertExtent;
        float maxY = camPos.y + vertExtent;

        // カメラ範囲内にClamp
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }
}
