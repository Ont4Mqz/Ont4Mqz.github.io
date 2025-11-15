using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int x, y;
    public int id;
    public bool match = false;

    private BoardManager board;
    public RectTransform Rect { get; private set; }

    Vector2 startDragPos;

    public void Init(BoardManager b, int px, int py, Sprite sprite)
    {
        board = b;
        x = px;
        y = py;

        id = sprite.GetHashCode();
        GetComponent<Image>().sprite = sprite;

        Rect = GetComponent<RectTransform>();
    }

    public void SetPos(int px, int py)
    {
        x = px;
        y = py;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 diff = eventData.position - startDragPos;

        // 移動方向を判定（縦横の大きい方）
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            // 横
            if (diff.x > 50) TrySwap(1, 0);
            else if (diff.x < -50) TrySwap(-1, 0);
        }
        else
        {
            // 縦
            if (diff.y > 50) TrySwap(0, 1);
            else if (diff.y < -50) TrySwap(0, -1);
        }
    }

    void TrySwap(int dx, int dy)
    {
        int nx = x + dx;
        int ny = y + dy;

        // 範囲チェック
        // BoardManager の width/height を使いたいが private の場合は public に変更しても良い
        // ここでは簡易に範囲外を無視
        if (nx < 0 || ny < 0) return;

        board.Swap(this, board.GetGem(nx, ny));
    }
}
