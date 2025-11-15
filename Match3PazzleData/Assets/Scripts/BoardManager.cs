using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    public int width = 6;
    public int height = 8;
    public float cellSize = 100f;

    [Header("UI Parent")]
    public RectTransform boardParent;

    [Header("Gem Prefab")]
    public GameObject gemPrefab;

    [Header("Gem Sprites")]
    public Sprite[] gemSprites;

    private Gem[,] gems;

    void Start()
    {
        CreateBoard();
    }

    void CreateBoard()
    {
        gems = new Gem[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnGem(x, y);
            }
        }
    }

    public void SpawnGem(int x, int y)
    {
        GameObject g = Instantiate(gemPrefab, boardParent);
        RectTransform rt = g.GetComponent<RectTransform>();

        float cell = cellSize;

        // 左上基準
        Vector2 offset = new Vector2(
            -(width * cell) / 2f + cell / 2f,
            (height * cell) / 2f - cell / 2f
        );

        rt.anchoredPosition = new Vector2(
            offset.x + x * cell,
            offset.y - y * cell
        );

        Gem gem = g.GetComponent<Gem>();
        gem.Init(this, x, y, gemSprites[Random.Range(0, gemSprites.Length)]);

        gems[x, y] = gem;
    }


    // -----------------------------
    // スワップ処理
    // -----------------------------
    public void Swap(Gem a, Gem b)
    {
        StartCoroutine(SwapRoutine(a, b));
    }

    IEnumerator SwapRoutine(Gem a, Gem b)
    {
        Vector2 aPos = a.Rect.anchoredPosition;
        Vector2 bPos = b.Rect.anchoredPosition;

        // 交換アニメ
        float t = 0;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            float p = t / 0.15f;

            a.Rect.anchoredPosition = Vector2.Lerp(aPos, bPos, p);
            b.Rect.anchoredPosition = Vector2.Lerp(bPos, aPos, p);

            yield return null;
        }

        // 位置入れ替え
        int ax = a.x;
        int ay = a.y;

        a.SetPos(b.x, b.y);
        b.SetPos(ax, ay);

        gems[a.x, a.y] = a;
        gems[b.x, b.y] = b;

        // マッチ判定
        if (!CheckMatches())
        {
            // 元に戻す
            StartCoroutine(SwapRoutine(b, a));
        }
        else
        {
            yield return StartCoroutine(DestroyMatches());
            yield return StartCoroutine(FallDown());
            yield return StartCoroutine(FillBoard());
        }
    }

    // -----------------------------
    // マッチ処理
    // -----------------------------
    bool CheckMatches()
    {
        bool matched = false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Gem gem = gems[x, y];
                if (gem == null) continue;

                // 横3
                if (x < width - 2)
                {
                    if (gems[x + 1, y].id == gem.id && gems[x + 2, y].id == gem.id)
                    {
                        gem.match = true;
                        gems[x + 1, y].match = true;
                        gems[x + 2, y].match = true;
                        matched = true;
                    }
                }

                // 縦3
                if (y < height - 2)
                {
                    if (gems[x, y + 1].id == gem.id && gems[x, y + 2].id == gem.id)
                    {
                        gem.match = true;
                        gems[x, y + 1].match = true;
                        gems[x, y + 2].match = true;
                        matched = true;
                    }
                }
            }
        }

        return matched;
    }

    IEnumerator DestroyMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gems[x, y] != null && gems[x, y].match)
                {
                    Destroy(gems[x, y].gameObject);
                    gems[x, y] = null;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator FallDown()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                if (gems[x, y] == null)
                {
                    for (int ny = y; ny < height; ny++)
                    {
                        if (gems[x, ny] != null)
                        {
                            gems[x, y] = gems[x, ny];
                            gems[x, ny] = null;

                            gems[x, y].SetPos(x, y);
                            UpdateGemPosition(gems[x, y]);
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator FillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gems[x, y] == null)
                {
                    SpawnGem(x, y);
                }
            }
        }
        yield return new WaitForSeconds(0.1f);

        // さらにマッチがあれば連鎖
        if (CheckMatches())
        {
            yield return StartCoroutine(DestroyMatches());
            yield return StartCoroutine(FallDown());
            yield return StartCoroutine(FillBoard());
        }
    }

    void UpdateGemPosition(Gem gem)
    {
        float cell = cellSize;
        Vector2 offset = new Vector2(
            -(width * cell) / 2f + cell / 2f,
            (height * cell) / 2f - cell / 2f
        );

        gem.Rect.anchoredPosition = new Vector2(
            offset.x + gem.x * cell,
            offset.y - gem.y * cell
        );
    }


    public Gem GetGem(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return null;
        return gems[x, y];
    }

}
