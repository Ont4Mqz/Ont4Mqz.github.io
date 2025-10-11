using UnityEngine;

public class BoomerangManager2D : MonoBehaviour
{
    [Header("ブーメラン設定")]
    public Transform player;                // プレイヤー
    public Transform boomerang;             // シーン上のブーメラン
    public float speed = 5f;                // 飛行速度
    public float rotationSpeed = 720f;      // 回転速度

    private Vector3 startPoint;             // 出発地点
    private Vector3 targetPoint;            // 目標地点
    private float journeyTime;              // 経過時間
    private bool isFlying = false;          // 飛行中か
    private bool isReturning = false;       // 戻り中か

    void Update()
    {
        // クリックで投げる
        if (Input.GetMouseButtonDown(0) && !isFlying)
        {
            startPoint = boomerang.position;                            // 出発点を記録
            targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // クリック位置取得
            targetPoint.z = 0;                                          // 2D用
            journeyTime = 0f;                                           // リセット
            isFlying = true;                                            // 飛行ON
            isReturning = false;                                        // 戻りOFF
        }

        if (isFlying)
        {
            MoveBoomerang(); // 飛行中処理
        }
        else
        {
            // プレイヤー手元に即追従
            boomerang.position = player.position + new Vector3(0.5f, 0.5f, 0);
        }
    }

    void MoveBoomerang()
    {
        journeyTime += Time.deltaTime * speed; // 時間進行
        float t = Mathf.Clamp01(journeyTime);  // tを0〜1に固定（暴走防止）

        if (!isReturning)
        {
            // 行き
            boomerang.position = Vector3.Lerp(startPoint, targetPoint, t); // 移動
            boomerang.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // 回転

            // 到達判定
            if (Vector2.Distance(boomerang.position, targetPoint) < 0.1f)
            {
                isReturning = true;
                journeyTime = 0f;
                startPoint = boomerang.position;
            }
        }
        else
        {
            // 帰り
            Vector3 currentTarget = player.position;                     // 動いてるプレイヤーに追従
            boomerang.position = Vector3.Lerp(startPoint, currentTarget, t);
            boomerang.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // 戻ったら終了
            if (Vector2.Distance(boomerang.position, currentTarget) < 0.2f)
            {
                isFlying = false;
            }
        }
    }
}
