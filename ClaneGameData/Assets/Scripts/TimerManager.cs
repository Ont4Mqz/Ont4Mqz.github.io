using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [SerializeField] private float limitTime = 30f; // 制限時間
    [SerializeField] private TextMeshProUGUI timerText; // タイマー表示UI

    private float currentTime; // 現在の時間

    private void Awake()
    {
        Instance = this;
    }

    void Start() // タイマー初期化
    {
        currentTime = limitTime;
    }

    void Update() // タイマー更新
    {
        currentTime -= Time.deltaTime;
        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
        {
            currentTime = 0;
            Time.timeScale = 0;
        }
    }

    public float GetTime() => currentTime;
}
