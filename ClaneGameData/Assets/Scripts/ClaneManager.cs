using System.Collections;
using UnityEngine;

public class ClaneManager : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("移動制限")]
    [SerializeField] private Vector2 minLimit;
    [SerializeField] private Vector2 maxLimit;

    [Header("リリース位置")]
    [SerializeField] private Vector2 releasePosition;

    [Header("アーム設定")]
    [SerializeField] private Transform armLeft;
    [SerializeField] private Transform armRight;

    [SerializeField] private float openAngle = 40f;
    [SerializeField] private float closeAngle = 5f;
    [SerializeField] private float armRotateSpeed = 120f;

    [Header("自動制御")]
    [SerializeField] private float autoMoveSpeed = 3f;
    [SerializeField] private float releaseDelay = 0.3f;

    private bool isCatching = false;
    private bool isAutoMoving = false;
    private float currentAngle;

    void Start()
    {
        currentAngle = openAngle;
        SetArmAngle(currentAngle);
    }

    void Update()
    {
        if (!isAutoMoving)
        {
            Move();
            CatchInput();
        }

        UpdateArms();
    }

    // --------------------
    // 手動移動
    // --------------------
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, y, 0) * moveSpeed * Time.deltaTime;
        transform.position += move;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minLimit.x, maxLimit.x);
        pos.y = Mathf.Clamp(pos.y, minLimit.y, maxLimit.y);
        transform.position = pos;
    }

    // --------------------
    // キャッチ入力
    // --------------------
    void CatchInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCatching = true;
            StartCoroutine(AutoMoveRoutine());
        }
    }

    // --------------------
    // アーム制御
    // --------------------
    void UpdateArms()
    {
        float targetAngle = isCatching ? closeAngle : openAngle;

        currentAngle = Mathf.MoveTowards(
            currentAngle,
            targetAngle,
            armRotateSpeed * Time.deltaTime
        );

        SetArmAngle(currentAngle);
    }

    void SetArmAngle(float angle)
    {
        armLeft.localRotation = Quaternion.Euler(0, 0, angle);
        armRight.localRotation = Quaternion.Euler(0, 0, -angle);
    }

    // --------------------
    // 自動搬送（Y → X → リリース）
    // --------------------
    IEnumerator AutoMoveRoutine()
    {
        isAutoMoving = true;

        // 掴む演出待ち
        yield return new WaitForSeconds(0.2f);

        // ① Y方向に移動
        while (Mathf.Abs(transform.position.y - releasePosition.y) > 0.05f)
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

        // ② X方向に移動
        while (Mathf.Abs(transform.position.x - releasePosition.x) > 0.05f)
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

        // リリース
        yield return new WaitForSeconds(releaseDelay);
        isCatching = false;

        yield return new WaitForSeconds(0.2f);
        isAutoMoving = false;
    }

    // --------------------
    // ギズモ表示
    // --------------------
    private void OnDrawGizmosSelected()
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
