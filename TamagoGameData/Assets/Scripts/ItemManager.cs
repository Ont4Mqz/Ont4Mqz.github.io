using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemEffect                                    // アイテム効果のデータ
    {
        public string itemTag;                                 // アイテムのタグ
        public float addSizeX;                                 // 増やすサイズ
    }

    [SerializeField] private ItemEffect[] itemEffects;          // アイテム効果の一覧

    public void ApplyItem(GameObject item)                      // アイテムが落とされた時の処理
    {
        foreach (var effect in itemEffects)                    // 全ての効果を調べる
        {
            if (item.CompareTag(effect.itemTag))               // タグが一致したか確認する
            {
                Vector3 scale = transform.localScale;          // 現在のサイズを取得する
                scale.x += effect.addSizeX;                    // 横幅を増やす
                transform.localScale = scale;                  // 新しいサイズを適用する
                return;                                        // 効果を適用したので終了する
            }
        }
    }
}
