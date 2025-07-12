using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    [Header("スコアテキスト")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("ノーツごとのスコア設定")]
    [SerializeField] private int scoreForNotesA = 100;
    [SerializeField] private int scoreForNotesD = 150;

    private int totalScore = 0;

    public void AddScore(GameObject note)
    {
        int scoreToAdd = 0;

        if (note.name.StartsWith("NotesA"))
        {
            scoreToAdd = scoreForNotesA;
        }
        else if (note.name.StartsWith("NotesD"))
        {
            scoreToAdd = scoreForNotesD;
        }
        else
        {
            Debug.LogWarning($"未対応のノーツ: {note.name}");
            return;
        }

        totalScore += scoreToAdd;
        UpdateScoreText();
        Debug.Log($"スコア加算: {scoreToAdd}（合計: {totalScore}）");
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {totalScore}";
        }
    }
}
