using UnityEngine;
using TMPro;
using System.Collections;

public class TimerManager : MonoBehaviour
{
    [Header("ゲーム制限時間")]
    [SerializeField] private float gameTimeLimit = 60f;

    [Header("レベルアップ設定")]
    [SerializeField] private float levelDuration = 10f;

    [Header("ゲーム開始カウントダウン")]
    [SerializeField] private float startCountDownTime = 3f;

    [Header("表示")]
    [SerializeField] private TMP_Text gameTimeText;
    [SerializeField] private TMP_Text levelTimerText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text startCountText;

    private float remainingGameTime;
    private float remainingLevelTime;
    private int speedLevel = 1;

    private bool isGameStarted = false;
    private bool isGameOver = false;

    public int SpeedLevel => speedLevel;
    public bool IsGameStarted => isGameStarted;
    public bool IsGameOver => isGameOver;

    public void StartGameCountdown()
    {
        StartCoroutine(GameStartCountDown());
    }

    IEnumerator GameStartCountDown()
    {
        float time = startCountDownTime;

        while (time > 0f)
        {
            startCountText.text = Mathf.CeilToInt(time).ToString();
            time -= Time.deltaTime;
            yield return null;
        }

        startCountText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        startCountText.text = "";

        StartGame();
    }

    void StartGame()
    {
        isGameStarted = true;
        remainingGameTime = gameTimeLimit;
        remainingLevelTime = levelDuration;
        UpdateUI();
    }

    void Update()
    {
        if (!isGameStarted || isGameOver) return;

        // ゲーム制限時間
        remainingGameTime -= Time.deltaTime;
        if (remainingGameTime <= 0f)
        {
            remainingGameTime = 0f;
            isGameOver = true;
        }

        // レベルアップ
        remainingLevelTime -= Time.deltaTime;
        if (remainingLevelTime <= 0f)
        {
            speedLevel++;
            remainingLevelTime = levelDuration;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        gameTimeText.text = Mathf.CeilToInt(remainingGameTime).ToString();
        levelTimerText.text = Mathf.CeilToInt(remainingLevelTime).ToString();
        levelText.text = $"LV {speedLevel}";
    }
}
