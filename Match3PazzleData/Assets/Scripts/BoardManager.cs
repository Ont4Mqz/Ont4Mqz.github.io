using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [Header("Board Size")]
    public int width = 8;
    public int height = 8;

    [Header("UI Board Panel (RectTransform)")]
    public RectTransform boardArea;

    [Header("Gem UI Prefab")]
    public Gem gemPrefab;

    [Header("Sprites")]
    public Sprite[] gemSprites;

    private Gem[,] board;
    private float cellSize;

    void Start()
    {
        board = new Gem[width, height];

        // セルのサイズを自動調整
        cellSize = boardArea.rect.width / width;

        CreateBoard();
    }

    void CreateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnGem(x, y);
            }
        }
    }

    // ======================
    // 生成
    // ======================

    void SpawnGem(int x, int y)
    {
        Gem g = Instantiate(gemPrefab, boardArea);
        int type = Random.Range(0, gemSprites.Length);

        g.GetComponent<Image>().sprite = gemSprites[type];
        g.Init(x, y, type, this);

        // 位置セット
        var rt = g.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(cellSize, cellSize);
        rt.anchoredPosition = GetPos(x, y);

        board[x, y] = g;
    }

    Vector2 GetPos(int x, int y)
    {
        return new Vector2(x * cellSize, y * cellSize);
    }

    public void ResetGemPosition(Gem g)
    {
        g.GetComponent<RectTransform>().anchoredPosition = GetPos(g.x, g.y);
    }

    // ======================
    // スワップ
    // ======================

    public void TrySwap(Gem g, int dx, int dy)
    {
        int tx = g.x + dx;
        int ty = g.y + dy;

        if (tx < 0 || tx >= width || ty < 0 || ty >= height)
        {
            ResetGemPosition(g);
            return;
        }

        Gem target = board[tx, ty];

        StartCoroutine(SwapRoutine(g, target));
    }

    IEnumerator SwapRoutine(Gem a, Gem b)
    {
        Swap(a, b);

        yield return new WaitForSeconds(0.05f);

        var matches = FindMatches();
        if (matches.Count == 0)
        {
            Swap(a, b); // 戻す
        }
        else
        {
            yield return ClearAndDrop(matches);
        }
    }

    void Swap(Gem a, Gem b)
    {
        // board 配列入れ替え
        board[a.x, a.y] = b;
        board[b.x, b.y] = a;

        // x y 入れ替え
        int ax = a.x; int ay = a.y;
        a.x = b.x; a.y = b.y;
        b.x = ax; b.y = ay;

        // 位置更新
        a.GetComponent<RectTransform>().anchoredPosition = GetPos(a.x, a.y);
        b.GetComponent<RectTransform>().anchoredPosition = GetPos(b.x, b.y);
    }

    // ======================
    // マッチ判定
    // ======================

    List<Gem> FindMatches()
    {
        List<Gem> matches = new List<Gem>();

        // 横
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                Gem a = board[x, y];
                Gem b = board[x + 1, y];
                Gem c = board[x + 2, y];

                if (a.type == b.type && b.type == c.type)
                {
                    matches.Add(a);
                    matches.Add(b);
                    matches.Add(c);
                }
            }
        }

        // 縦
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                Gem a = board[x, y];
                Gem b = board[x, y + 1];
                Gem c = board[x, y + 2];

                if (a.type == b.type && b.type == c.type)
                {
                    matches.Add(a);
                    matches.Add(b);
                    matches.Add(c);
                }
            }
        }

        return matches;
    }

    // ======================
    // 消す → 落下 → 補充
    // ======================

    IEnumerator ClearAndDrop(List<Gem> matches)
    {
        // 消去
        foreach (var g in matches)
        {
            board[g.x, g.y] = null;
            Destroy(g.gameObject);
        }

        yield return new WaitForSeconds(0.05f);

        // 落下
        for (int x = 0; x < width; x++)
        {
            int emptyY = 0;

            for (int y = 0; y < height; y++)
            {
                if (board[x, y] == null)
                {
                    emptyY = y;

                    // 上から探す
                    for (int ny = y + 1; ny < height; ny++)
                    {
                        if (board[x, ny] != null)
                        {
                            Gem g = board[x, ny];

                            board[x, emptyY] = g;
                            board[x, ny] = null;

                            g.y = emptyY;
                            g.GetComponent<RectTransform>().anchoredPosition = GetPos(x, emptyY);

                            break;
                        }
                    }
                }
            }
        }

        // 補充
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (board[x, y] == null)
                {
                    SpawnGem(x, y);
                }
            }
        }

        yield return new WaitForSeconds(0.05f);

        // 連鎖チェック
        var next = FindMatches();
        if (next.Count > 0)
        {
            yield return ClearAndDrop(next);
        }
    }
}
