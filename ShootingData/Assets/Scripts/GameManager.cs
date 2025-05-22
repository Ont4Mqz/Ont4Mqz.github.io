using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("�Q�[���I�[�o�[����p�g���K�[�Q")]
    public Collider2D[] gameOverTriggers;

    [Header("�Q�[���I�[�o�[�ΏۃI�u�W�F�N�g�Q")]
    public Collider2D[] gameOverTargets;

    [Header("�Q�[���N���A����p�g���K�[�Q")]
    public Collider2D[] gameClearTriggers;

    [Header("�Q�[���N���A�ΏۃI�u�W�F�N�g�Q")]
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
                    Debug.Log($"[GameOver����] '{target.name}' �� '{trigger.name}' �ɐڐG���܂����B");
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
                    Debug.Log($"[GameClear����] '{target.name}' �� '{trigger.name}' �ɐڐG���܂����B");
                    SceneManager.LoadScene("GameClear");
                }
            }
        }
    }
}
