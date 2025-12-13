using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [SerializeField] private float limitTime = 30f;
    [SerializeField] private TextMeshProUGUI timerText;

    private float currentTime;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTime = limitTime;
    }

    void Update()
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
