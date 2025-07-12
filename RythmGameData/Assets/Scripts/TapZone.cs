using UnityEngine;
using System.Collections.Generic;

public class TapZone : MonoBehaviour
{
    [Header("対応するNotesAとNotesDのプレハブ（参照用）")]
    [SerializeField] private GameObject notesAPrefab;
    [SerializeField] private GameObject notesDPrefab;

    [Header("スコアマネージャー")]
    [SerializeField] private ScoreManager scoreManager;

    private List<GameObject> notesInZone = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryDestroyNote(notesAPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryDestroyNote(notesDPrefab);
        }
    }

    private void TryDestroyNote(GameObject targetPrefab)
    {
        for (int i = 0; i < notesInZone.Count; i++)
        {
            GameObject note = notesInZone[i];

            if (note != null && note.name.StartsWith(targetPrefab.name))
            {
                Debug.Log($"キー押下: {targetPrefab.name} に対応するノートを破壊");

                // スコア加算
                if (scoreManager != null)
                {
                    scoreManager.AddScore(note);
                }

                Destroy(note);
                notesInZone.RemoveAt(i);
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!notesInZone.Contains(other.gameObject))
        {
            notesInZone.Add(other.gameObject);
            Debug.Log($"ノートが判定ゾーンに入りました: {other.gameObject.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (notesInZone.Contains(other.gameObject))
        {
            notesInZone.Remove(other.gameObject);
            Debug.Log($"ノートが判定ゾーンを出ました: {other.gameObject.name}");
        }
    }
}
