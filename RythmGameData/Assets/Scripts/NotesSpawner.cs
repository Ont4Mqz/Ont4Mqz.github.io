using UnityEngine;
using UnityEngine.UI;

public class NotesSpawner : MonoBehaviour
{
    [Header("ノーツのプレハブ（複数）")]
    [SerializeField] private GameObject[] notePrefabs;

    [Header("スポーンする親Canvas")]
    [SerializeField] private RectTransform spawnCanvas;

    [Header("スポーン間隔（秒）")]
    [SerializeField] private float spawnInterval = 1.0f;

    private float timer = 0f;
    private RectTransform spawnerRect;

    private void Start()
    {
        spawnerRect = GetComponent<RectTransform>();
        if (spawnerRect == null)
        {
            Debug.LogError("NotesSpawner には RectTransform が必要です（Canvas上のUIとして配置してください）");
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnNote();
            timer = 0f;
        }
    }

    private void SpawnNote()
    {
        if (spawnerRect == null) return;

        // ランダムなプレハブを選択
        int randomIndex = Random.Range(0, notePrefabs.Length);
        GameObject selectedPrefab = notePrefabs[randomIndex];

        // プレハブを生成し、Canvas の子に設定
        GameObject spawnedNote = Instantiate(selectedPrefab, spawnCanvas);

        // RectTransformを取得し、Spawner と同じ位置に配置
        RectTransform noteRect = spawnedNote.GetComponent<RectTransform>();
        if (noteRect != null)
        {
            noteRect.anchoredPosition = spawnerRect.anchoredPosition;
        }
        else
        {
            Debug.LogWarning("Spawned note does not have a RectTransform component.");
        }
    }
}
