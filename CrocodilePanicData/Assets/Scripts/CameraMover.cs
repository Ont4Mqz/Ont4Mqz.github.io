using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform targetPos; // 移動先
    [SerializeField] private float moveTime = 3f; // 移動にかける時間

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (timer < moveTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveTime);
            t = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.Lerp(startPos, targetPos.position, t);
        }
    }
}
