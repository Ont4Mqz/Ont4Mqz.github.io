using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [Header("スポーン候補プレハブ")]
    [SerializeField] private List<GameObject> spawnCandidates; // スポーン候補プレハブリスト

    [Header("ItemSpawner")]
    [SerializeField] private ItemSpawner itemSpawner; // アイテムスポーナー

    [Header("最初に湧く個数")]
    [SerializeField] private int initialSpawnCount = 6; // 最初にスポーンする個数

    [Header("UI（3種アイコン）")]
    [SerializeField] private Image[] itemIcons; // アイテムアイコンUI配列

    [Header("次の補充数UI")]
    [SerializeField] private TextMeshProUGUI nextSpawnCountText; // 次の補充数表示UI

    [Header("補充数の範囲")]
    [SerializeField] private int minNextSpawn = 3; // 最小補充数
    [SerializeField] private int maxNextSpawn = 8; // 最大補充数

    [Header("ターゲット更新間隔")]
    [SerializeField] private float updateInterval = 10f; // ターゲット更新間隔

    [Header("色設定")]
    [SerializeField] private Color lockedColor = Color.black; // ロック中のアイコン色

    private HashSet<int> targetIDs = new(); // 目標アイテムIDセット
    private HashSet<int> droppedIDs = new(); // ドロップ済みアイテムIDセット
    private int nextSpawnCount; // 次の補充数

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        itemSpawner.SpawnRandom(spawnCandidates, initialSpawnCount); // 最初のアイテムスポーン

        UpdateTargets();
        StartCoroutine(UpdateTimerRoutine()); // ターゲット更新コルーチン開始
    }

    IEnumerator UpdateTimerRoutine() // ターゲット更新コルーチン
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            UpdateTargets();
        }
    }

    void UpdateTargets()
    {
        targetIDs.Clear();
        droppedIDs.Clear();

        nextSpawnCount = Random.Range(minNextSpawn, maxNextSpawn + 1); // 次の補充数決定
        nextSpawnCountText.text = $"{nextSpawnCount}"; // 次の補充数表示更新

        List<GameObject> shuffled = new(spawnCandidates); // スポーン候補をシャッフル
        shuffled.Sort((a, b) => Random.Range(-1, 2)); // ランダムソート

        for (int i = 0; i < 3; i++) // 目標3種設定
        {
            ItemData data = shuffled[i].GetComponent<ItemData>();
            targetIDs.Add(data.itemID);

            itemIcons[i].sprite =
                shuffled[i].GetComponent<SpriteRenderer>().sprite;
            itemIcons[i].color = lockedColor;
        }
    }

    public void OnItemDropped(ItemData data) // アイテムドロップ時処理
    {
        ScoreManager.Instance.AddScore(data.score); // スコア加算

        if (!targetIDs.Contains(data.itemID))
            return;

        if (droppedIDs.Contains(data.itemID))
            return;

        droppedIDs.Add(data.itemID); // ドロップ済みIDに追加
        UnlockIcon(data.itemID);

        if (droppedIDs.Count == 3)
        {
            StartCoroutine(SpawnAndRefresh()); // アイテム補充＆ターゲット更新コルーチン開始
        }
    }

    IEnumerator SpawnAndRefresh() // アイテム補充＆ターゲット更新コルーチン
    {
        foreach (int id in targetIDs)
        {
            GameObject prefab = spawnCandidates.Find(
                x => x.GetComponent<ItemData>().itemID == id // アイテムIDでプレハブ検索
            );
            itemSpawner.Spawn(prefab, 1);
        }

        int remaining = nextSpawnCount - 3;
        if (remaining > 0)
        {
            itemSpawner.SpawnRandom(spawnCandidates, remaining);
        }

        yield return new WaitForSeconds(0.3f); // 少し待つ
        UpdateTargets();
    }

    void UnlockIcon(int id) // アイコンロック解除
    {
        int index = 0;
        foreach (int targetID in targetIDs) // 目標IDを走査
        {
            if (targetID == id)
            {
                itemIcons[index].color = Color.white; // ロック解除
                return;
            }
            index++;
        }
    }
}
