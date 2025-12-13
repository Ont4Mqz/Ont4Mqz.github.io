using UnityEngine;

public class ItemData : MonoBehaviour
{
    public int itemID; // アイテムID
    public int score; // アイテムのスコア値

    private void OnTriggerEnter2D(Collider2D other) // ゴール判定
    {
        if (other.CompareTag("Goal"))
        {
            ItemManager.Instance.OnItemDropped(this);
            Destroy(gameObject);
        }
    }
}
