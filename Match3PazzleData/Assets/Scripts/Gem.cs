using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public int type;
    public int x;
    public int y;

    private BoardManager board;
    private RectTransform rect;
    private Vector2 startPos;
    private Vector2 pointerDownPos;

    public void Init(int _x, int _y, int _type, BoardManager _board)
    {
        x = _x;
        y = _y;
        type = _type;
        board = _board;
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 見た目を引っ張る（任意）
        rect.anchoredPosition += eventData.delta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 diff = eventData.position - pointerDownPos;

        // どの方向にドラッグしたか判定
        if (diff.magnitude < 30f)
        {
            // 小さいドラッグは無視して元に戻す
            board.ResetGemPosition(this);
            return;
        }

        int dx = 0;
        int dy = 0;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            dx = diff.x > 0 ? 1 : -1;
        }
        else
        {
            dy = diff.y > 0 ? 1 : -1;
        }

        board.TrySwap(this, dx, dy);
    }
}
