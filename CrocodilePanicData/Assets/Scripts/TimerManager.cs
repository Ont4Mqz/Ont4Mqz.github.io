using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [Header("レベルアップ設定")]
    [SerializeField] private float levelDuration = 10f;

    [Header("表示")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text levelText;

    private float remainingTime;
    private int speedLevel = 1;

    public int SpeedLevel => speedLevel;

    void Start()
    {
        remainingTime = levelDuration;
        UpdateUI();
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            speedLevel++;
            remainingTime = levelDuration;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();
        }

        if (levelText != null)
        {
            levelText.text = $"LV {speedLevel}";
        }
    }
}
