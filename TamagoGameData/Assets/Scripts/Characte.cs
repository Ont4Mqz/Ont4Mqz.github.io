using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float minMoveSpeed = 0.5f;     // 最低の歩く速さ
    [SerializeField] private float maxMoveSpeed = 1.5f;     // 最大の歩く速さ
    [SerializeField] private float minMoveDuration = 1f;    // 最短の移動時間
    [SerializeField] private float maxMoveDuration = 3f;    // 最長の移動時間
    [SerializeField] private float minStopDuration = 1f;    // 最短の停止時間
    [SerializeField] private float maxStopDuration = 3f;    // 最長の停止時間

    private Vector2 floorMin;                               // 床の左下の位置
    private Vector2 floorMax;                               // 床の右上の位置
    private Vector2 targetPos;                              // 次に向かう位置
    private float moveSpeed;                                // 今の移動速度
    private bool isMoving = false;                          // 動いているかどうか

    private void Start()
    {
        DetectFloorArea();                                  // 床の範囲を調べる
        StartCoroutine(MovementLoop());                     // 行動ループを開始する
    }

    private void DetectFloorArea()
    {
        Collider2D floor = GameObject.FindGameObjectWithTag("Floor").GetComponent<Collider2D>(); // Floorのコライダーを取得する
        Bounds b = floor.bounds;                                                                 // 範囲を取り出す
        floorMin = b.min;                                                                        // 左下座標を保存する
        floorMax = b.max;                                                                        // 右上座標を保存する
    }

    private System.Collections.IEnumerator MovementLoop()
    {
        while (true)                                                                              // 行動を繰り返す
        {
            float stopTime = Random.Range(minStopDuration, maxStopDuration);                      // 止まる時間を決める
            isMoving = false;                                                                     // 停止状態にする
            yield return new WaitForSeconds(stopTime);                                            // 止まる時間だけ待つ

            isMoving = true;                                                                      // 動く状態にする
            moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);                                 // 速度を決める

            targetPos = new Vector2(                                                              // 向かう位置を決める
                Random.Range(floorMin.x, floorMax.x),                                             // ランダムなX
                Random.Range(floorMin.y, floorMax.y)                                              // ランダムなY
            );

            float moveTime = Random.Range(minMoveDuration, maxMoveDuration);                      // 動く時間を決める
            float timer = 0f;                                                                     // 経過時間をゼロにする

            while (timer < moveTime)                                                              // 動く時間の間だけ移動する
            {
                timer += Time.deltaTime;                                                          // 経過時間を増やす

                transform.position = Vector2.MoveTowards(                                         // 位置を少しずつ動かす
                    transform.position,                                                           // 現在の位置
                    targetPos,                                                                     // 目的地
                    moveSpeed * Time.deltaTime                                                     // 移動量
                );

                yield return null;                                                                // 次のフレームまで待つ
            }
        }
    }
}
