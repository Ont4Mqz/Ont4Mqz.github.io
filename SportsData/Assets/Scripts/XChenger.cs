using UnityEngine;

public class XChanger : MonoBehaviour
{
    public float rotationSpeed = 100f; // 回転速度（度/秒）

    void Update()
    {
        float xRotation = 0f;

        // Aキーで負の方向に回転
        if (Input.GetKey(KeyCode.A))
        {
            xRotation = -rotationSpeed * Time.deltaTime;
        }
        // Dキーで正の方向に回転
        else if (Input.GetKey(KeyCode.D))
        {
            xRotation = rotationSpeed * Time.deltaTime;
        }

        // ローカルX軸回転を加える
        transform.Rotate(xRotation, 0f, 0f, Space.Self);
    }
}
