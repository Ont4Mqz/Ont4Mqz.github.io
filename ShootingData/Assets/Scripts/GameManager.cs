using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("ゲームオーバー判定用トリガー群")]
    public Collider2D[] gameOverTriggers;

    [Header("ゲームオーバー対象オブジェクト群")]
    public Collider2D[] gameOverTargets;

    [Header("ゲームクリア判定用トリガー群")]
    public Collider2D[] gameClearTriggers;

    [Header("ゲームクリア対象オブジェクト群")]
    public Collider2D[] gameClearTargets;

    void Update()
    {
        CheckGameOver();
        CheckGameClear();
    }

    void CheckGameOver()
    {
        foreach (var trigger in gameOverTriggers)
        {
            if (trigger == null) continue;

            foreach (var target in gameOverTargets)
            {
                if (target == null) continue;

                if (trigger.bounds.Intersects(target.bounds))
                {
                    Debug.Log($"[GameOver判定] '{target.name}' が '{trigger.name}' に接触しました。");
                    SceneManager.LoadScene("GameOver");
                }
            }
        }
    }

    void CheckGameClear()
    {
        foreach (var trigger in gameClearTriggers)
        {
            if (trigger == null) continue;

            foreach (var target in gameClearTargets)
            {
                if (target == null) continue;

                if (trigger.bounds.Intersects(target.bounds))
                {
                    Debug.Log($"[GameClear判定] '{target.name}' が '{trigger.name}' に接触しました。");
                    SceneManager.LoadScene("GameClear");
                }
            }
        }
    }
}
