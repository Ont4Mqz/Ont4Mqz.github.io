using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapManager : MonoBehaviour
{
    [Header("Tilemap 参照")]
    [SerializeField] private Tilemap groundTilemap;        // 地面タイル
    [SerializeField] private Tilemap wallTilemap;          // 壁タイル

    [Header("タイル")]
    [SerializeField] private TileBase groundTile;          // 床
    [SerializeField] private TileBase wallTile;            // 壁

    [Header("マップ設定")]
    [SerializeField] private int width = 60;               // 全体幅
    [SerializeField] private int height = 40;              // 全体高さ
    [SerializeField] private int roomCount = 6;            // 部屋数
    [SerializeField] private int roomMinSize = 4;          // 最小部屋サイズ
    [SerializeField] private int roomMaxSize = 10;         // 最大部屋サイズ
    [SerializeField] private int corridorWidth = 2;        // 通路幅

    [Header("Player スポーン設定")]
    [SerializeField] private GameObject playerPrefab;      // スポーンするプレイヤープレハブ

    private int[,] map;                                    // 0 = 壁, 1 = 床
    private List<Rect> rooms = new List<Rect>();           // 生成された部屋のリスト
    private List<Vector2Int> roomCenters = new List<Vector2Int>(); // 部屋の中心座標リスト

    void Start()
    {
        GenerateMap();                                     // マップ生成
        DrawMap();                                         // Tilemap に描画
        SpawnPlayer();                                     // プレイヤー生成
    }

    // =======================
    //  マップ生成
    // =======================
    void GenerateMap()
    {
        map = new int[width, height];
        rooms.Clear();
        roomCenters.Clear();

        // --- 部屋生成 ---
        for (int i = 0; i < roomCount; i++)
        {
            int rw = Random.Range(roomMinSize, roomMaxSize);
            int rh = Random.Range(roomMinSize, roomMaxSize);
            int rx = Random.Range(1, width - rw - 1);
            int ry = Random.Range(1, height - rh - 1);

            Rect newRoom = new Rect(rx, ry, rw, rh);

            // 重なりチェック
            bool overlaps = false;
            foreach (var room in rooms)
            {
                if (newRoom.Overlaps(room))
                {
                    overlaps = true;
                    break;
                }
            }
            if (overlaps) continue;

            rooms.Add(newRoom);

            // 部屋を書き込み
            for (int x = rx; x < rx + rw; x++)
            {
                for (int y = ry; y < ry + rh; y++)
                    map[x, y] = 1;
            }

            // 中心を保存
            roomCenters.Add(RoomCenter(newRoom));
        }

        // --- 通路生成 ---
        for (int i = 0; i < roomCenters.Count - 1; i++)
            CreateCorridor(roomCenters[i], roomCenters[i + 1]);
    }

    Vector2Int RoomCenter(Rect room)
    {
        int cx = Mathf.RoundToInt(room.x + room.width / 2);
        int cy = Mathf.RoundToInt(room.y + room.height / 2);
        return new Vector2Int(cx, cy);
    }

    void CreateCorridor(Vector2Int a, Vector2Int b)
    {
        // 横に掘る
        int xStart = Mathf.Min(a.x, b.x);
        int xEnd = Mathf.Max(a.x, b.x);

        for (int x = xStart; x <= xEnd; x++)
        {
            for (int w = -corridorWidth / 2; w <= corridorWidth / 2; w++)
            {
                int y = a.y + w;
                if (InMap(x, y)) map[x, y] = 1;
            }
        }

        // 縦に掘る
        int yStart = Mathf.Min(a.y, b.y);
        int yEnd = Mathf.Max(a.y, b.y);

        for (int y = yStart; y <= yEnd; y++)
        {
            for (int w = -corridorWidth / 2; w <= corridorWidth / 2; w++)
            {
                int x = b.x + w;
                if (InMap(x, y)) map[x, y] = 1;
            }
        }
    }

    bool InMap(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    // =======================
    //  Tilemap へ描画
    // =======================
    void DrawMap()
    {
        groundTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                if (map[x, y] == 1)
                    groundTilemap.SetTile(pos, groundTile); // 床
                else
                    wallTilemap.SetTile(pos, wallTile);     // 壁
            }
        }
    }

    // =======================
    //  Player をスポーン
    // =======================
    void SpawnPlayer()
    {
        if (playerPrefab == null || roomCenters.Count == 0)
        {
            Debug.LogWarning("PlayerPrefab が設定されていない、または部屋が無い");
            return;
        }

        // ランダムな部屋を選ぶ
        Vector2Int spawnPos = roomCenters[Random.Range(0, roomCenters.Count)];

        // Instantiate
        Instantiate(playerPrefab, new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
    }
}
