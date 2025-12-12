using UnityEngine;

public class RotateFloor : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float startRotationSpeed = 30f;      // 初期回転速度（度/秒）
    public float rotationSpeedIncrease = 2f;    // 1秒ごとの加速量

    private float currentSpeed;

    void Start()
    {
        currentSpeed = startRotationSpeed;
    }

    void Update()
    {
        // スピード上昇
        currentSpeed += rotationSpeedIncrease * Time.deltaTime;

        // 回転（Y軸）
        transform.Rotate(0, currentSpeed * Time.deltaTime, 0);
    }
}
