using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText; // スコア表示UI
    private int score; // スコア値

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int value) // スコア加算
    {
        score += value;
        scoreText.text = score.ToString();
    }
}
