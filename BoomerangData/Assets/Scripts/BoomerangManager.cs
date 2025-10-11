using UnityEngine;

public class BoomerangManager2D : MonoBehaviour
{
    [Header("ブーメラン設定")]
    public Transform player;                 // プレイヤー（帰還先）
    public Transform boomerang;              // シーン上のブーメラン
    public float speed = 5f;                 // 移動速度
    public float rotationSpeed = 720f;       // 回転速度（度/秒）

    private Vector3 startPoint;              // 出発地点
    private Vector3 targetPoint;             // クリック地点
    private float journeyTime = 0f;          // 経過時間
    private bool isFlying = false;           // 飛行中か
    private bool isReturning = false;        // 戻り中か

    void Update()
    {
        // 左クリックで投げる
        if (Input.GetMouseButtonDown(0) && !isFlying)
        {
            startPoint = boomerang.position;                              // 出発点
            targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // クリック位置を取得
            targetPoint.z = 0;                                            // 2Dなのでz固定
            isFlying = true;                                              // 飛行ON
            isReturning = false;                                          // 戻りOFF
            journeyTime = 0f;                                             // タイマーリセット
        }

        // 飛行中なら移動
        if (isFlying)
        {
            MoveBoomerang();
        }
        else
        {
            // プレイヤーの手元位置を常に追従（遅れなし）
            Vector3 handOffset = new Vector3(0.5f, 0.5f, 0); // 手元の相対位置
            boomerang.position = player.position + handOffset; // 直接追従
        }
    }

    void MoveBoomerang()
    {
        journeyTime += Time.deltaTime * speed; // 時間進行
        float t = journeyTime;

        if (!isReturning)
        {
            // 行きフェーズ
            if (t >= 1f)
            {
                // 目的地に到達 → 帰還へ
                isReturning = true;
                journeyTime = 0f;
                startPoint = boomerang.position;
                targetPoint = player.position;
                return;
            }

            // 線形に移動
            boomerang.position = Vector3.Lerp(startPoint, targetPoint, t);
            boomerang.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // 移動中のみ回転
        }
        else
        {
            // 帰還フェーズ
            if (t >= 1f)
            {
                // プレイヤーに戻ったら終了
                isFlying = false;
                return;
            }

            // プレイヤーの現在位置を常に更新（動いてても追う）
            Vector3 currentTarget = player.position;
            boomerang.position = Vector3.Lerp(startPoint, currentTarget, t);
            boomerang.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
}
