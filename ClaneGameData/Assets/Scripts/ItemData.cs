using UnityEngine;

public class ItemData : MonoBehaviour
{
    public int itemID;
    public int score;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            ItemManager.Instance.OnItemDropped(this);
            Destroy(gameObject);
        }
    }
}
