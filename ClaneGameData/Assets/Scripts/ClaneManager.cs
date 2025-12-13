using System.Collections;
using UnityEngine;

public class ClaneManager : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f; // 移動速度

    [Header("移動制限")]
    [SerializeField] private Vector2 minLimit; // 最小制限
    [SerializeField] private Vector2 maxLimit; // 最大制限

    [Header("リリース位置")]
    [SerializeField] private Vector2 releasePosition; // リリース位置

    [Header("アーム設定")]
    [SerializeField] private Transform armLeft; // 左アーム
    [SerializeField] private Transform armRight; // 

    [SerializeField] private float openAngle = 40f; // 開く角度
    [SerializeField] private float closeAngle = 5f; // 閉じる角度
    [SerializeField] private float armRotateSpeed = 120f; // アーム回転速度

    [Header("自動制御")]
    [SerializeField] private float autoMoveSpeed = 3f; // 自動移動速度
    [SerializeField] private float releaseDelay = 0.3f; // リリースまでの遅延時間

    private bool isCatching = false; // キャッチ中フラグ
    private bool isAutoMoving = false; // 自動移動中フラグ
    private float currentAngle; // 現在のアーム角度

    void Start()
    {
        currentAngle = openAngle;
        SetArmAngle(currentAngle);
    }

    void Update()
    {
        if (!isAutoMoving) // 自動移動中でなければ
        {
            Move();
            CatchInput();
        }

        UpdateArms();
    }

    void Move() // 手動移動
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, y, 0) * moveSpeed * Time.deltaTime;
        transform.position += move;

        Vector3 pos = transform.position; // 位置制限
        pos.x = Mathf.Clamp(pos.x, minLimit.x, maxLimit.x);
        pos.y = Mathf.Clamp(pos.y, minLimit.y, maxLimit.y);
        transform.position = pos;
    }

    void CatchInput() // キャッチ入力
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCatching = true;
            StartCoroutine(AutoMoveRoutine()); // 自動搬送開始
        }
    }

    void UpdateArms() // アームの開閉更新
    {
        float targetAngle = isCatching ? closeAngle : openAngle;

        currentAngle = Mathf.MoveTowards(
            currentAngle,
            targetAngle,
            armRotateSpeed * Time.deltaTime
        );

        SetArmAngle(currentAngle);
    }

    void SetArmAngle(float angle) // アーム角度設定
    {
        armLeft.localRotation = Quaternion.Euler(0, 0, angle);
        armRight.localRotation = Quaternion.Euler(0, 0, -angle);
    }

    IEnumerator AutoMoveRoutine() // 自動搬送コルーチン
    {
        isAutoMoving = true;

        yield return new WaitForSeconds(0.2f); // 少し待つ

        while (Mathf.Abs(transform.position.y - releasePosition.y) > 0.05f) // Y方向に移動
        {
            float y = Mathf.MoveTowards(
                transform.position.y,
                releasePosition.y,
                autoMoveSpeed * Time.deltaTime
            );

            transform.position = new Vector3(
                transform.position.x,
                y,
                transform.position.z
            );

            yield return null;
        }

        while (Mathf.Abs(transform.position.x - releasePosition.x) > 0.05f) // X方向に移動
        {
            float x = Mathf.MoveTowards(
                transform.position.x,
                releasePosition.x,
                autoMoveSpeed * Time.deltaTime
            );

            transform.position = new Vector3(
                x,
                transform.position.y,
                transform.position.z
            );

            yield return null;
        }

        yield return new WaitForSeconds(releaseDelay); // リリース待機
        isCatching = false;

        yield return new WaitForSeconds(0.2f); // 少し待つ
        isAutoMoving = false;
    }

    private void OnDrawGizmosSelected() // ギズモ表示
    {
        Gizmos.color = Color.cyan;

        Vector3 center = new Vector3(
            (minLimit.x + maxLimit.x) * 0.5f,
            (minLimit.y + maxLimit.y) * 0.5f,
            0
        );

        Vector3 size = new Vector3(
            maxLimit.x - minLimit.x,
            maxLimit.y - minLimit.y,
            0
        );

        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(releasePosition, 0.1f);
    }
}
