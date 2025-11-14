using UnityEngine;

public class AngleChanger : MonoBehaviour
{
    [Header("回転速度設定")]
    public float rotateSpeed = 90f; // 1秒に何度回すか

    void Update()
    {
        float y = 0f;

        // Aキー → 左回転（マイナス方向）
        if (Input.GetKey(KeyCode.A))
        {
            y -= rotateSpeed * Time.deltaTime;
        }

        // Dキー → 右回転（プラス方向）
        if (Input.GetKey(KeyCode.D))
        {
            y += rotateSpeed * Time.deltaTime;
        }

        // 回転を反映
        transform.Rotate(0f, y, 0f);
    }
}
