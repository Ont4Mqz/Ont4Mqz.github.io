using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [Header("達成条件用プレハブ")]
    [SerializeField] private List<GameObject> allItems;

    [Header("既存 ItemSpawner")]
    [SerializeField] private ItemSpawner itemSpawner;

    [Header("UI（3種アイコン）")]
    [SerializeField] private Image[] itemIcons;

    [Header("次の補充数UI")]
    [SerializeField] private TextMeshProUGUI nextSpawnCountText;

    [Header("補充数の範囲")]
    [SerializeField] private int minNextSpawn = 3;
    [SerializeField] private int maxNextSpawn = 8;

    [Header("ターゲット更新間隔")]
    [SerializeField] private float updateInterval = 10f;

    [Header("色設定")]
    [SerializeField] private Color lockedColor = Color.black;

    private List<GameObject> targetPrefabs = new();
    private HashSet<int> droppedIDs = new();

    private int nextSpawnCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (itemIcons.Length < 3)
        {
            Debug.LogError("ItemIcons は3個必要です");
            return;
        }

        UpdateTargets();
        StartCoroutine(UpdateTimerRoutine());
    }

    IEnumerator UpdateTimerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            UpdateTargets();
        }
    }

    // --------------------
    // ターゲット更新
    // --------------------
    void UpdateTargets()
    {
        targetPrefabs.Clear();
        droppedIDs.Clear();

        nextSpawnCount = Random.Range(minNextSpawn, maxNextSpawn + 1);
        nextSpawnCountText.text = $"Next Drops {nextSpawnCount}";

        List<GameObject> shuffled = new(allItems);
        shuffled.Sort((a, b) => Random.Range(-1, 2));

        for (int i = 0; i < 3; i++)
        {
            GameObject prefab = shuffled[i];
            targetPrefabs.Add(prefab);

            itemIcons[i].sprite =
                prefab.GetComponent<SpriteRenderer>().sprite;
            itemIcons[i].color = lockedColor;
        }
    }

    // --------------------
    // 落下通知
    // --------------------
    public void OnItemDropped(ItemData data)
    {
        ScoreManager.Instance.AddScore(data.score);

        // ターゲットか？
        foreach (var prefab in targetPrefabs)
        {
            if (prefab.GetComponent<ItemData>().itemID == data.itemID)
            {
                droppedIDs.Add(data.itemID);
                UnlockIcon(data.itemID);
                break;
            }
        }

        // ★ 3種すべて達成
        if (droppedIDs.Count >= 3)
        {
            SpawnWithGuarantee();
            UpdateTargets();
        }
    }

    // --------------------
    // 必須3種を必ず含めてスポーン
    // --------------------
    void SpawnWithGuarantee()
    {
        int remaining = nextSpawnCount;

        // ① 必須スポーン（各1）
        foreach (var prefab in targetPrefabs)
        {
            itemSpawner.Spawn(prefab, 1);
            remaining--;
        }

        // ② 残りを完全ランダム
        if (remaining > 0)
        {
            itemSpawner.SpawnRandom(remaining);
        }
    }

    void UnlockIcon(int id)
    {
        for (int i = 0; i < targetPrefabs.Count; i++)
        {
            if (targetPrefabs[i].GetComponent<ItemData>().itemID == id)
            {
                itemIcons[i].color = Color.white;
                return;
            }
        }
    }
}
