using UnityEngine;

public class DragItem : MonoBehaviour
{
    [SerializeField] private string targetTag = "Character";        // キャラのタグ
    private Vector3 startPos;                                       // 元の位置
    private bool isDragging = false;                                // ドラッグ中かどうか
    private Collider2D selfCol;                                     // 自分のコライダー

    private void Start()
    {
        startPos = transform.position;                              // 開始位置を保存する
        selfCol = GetComponent<Collider2D>();                       // 自分のコライダーを取得する
    }

    private void OnMouseDown()
    {
        isDragging = true;                                          // ドラッグ開始
    }

    private void OnMouseUp()
    {
        isDragging = false;                                         // ドラッグ終了
        StartCoroutine(DelayedDrop());                              // 次フレームで判定する
    }

    private void Update()
    {
        if (isDragging)                                             // ドラッグ中だけ動かす
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // マウス位置
            mousePos.z = 0f;                                        // Zを固定する
            transform.position = mousePos;                          // 位置を更新する
        }
    }

    private System.Collections.IEnumerator DelayedDrop()
    {
        yield return null;                                          // 1フレーム待つ

        Collider2D hit = Physics2D.OverlapPoint(transform.position); // ドロップ位置を判定する

        // ★ 自分自身のコライダーを無視する
        if (hit != null && hit != selfCol && hit.CompareTag(targetTag))
        {
            ItemManager manager = hit.GetComponent<ItemManager>();  // キャラのアイテム管理を取得する
            if (manager != null)
            {
                manager.ApplyItem(this.gameObject);                 // アイテム効果を渡す
            }

            Destroy(gameObject);                                    // アイテムを消す
        }
        else
        {
            transform.position = startPos;                          // 元の場所に戻す
        }
    }
}
