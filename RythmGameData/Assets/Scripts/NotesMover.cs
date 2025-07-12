using UnityEngine;

public class NotesMover : MonoBehaviour
{
    [Header("ノーツの移動速度（単位：UI単位/秒）")]
    [SerializeField] private float moveSpeed = 200f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("NotesMover を使用するには RectTransform が必要です。UIオブジェクトにアタッチしてください。");
        }
    }

    private void Update()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
        }
    }
}
