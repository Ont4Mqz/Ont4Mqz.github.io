using UnityEngine;
using TMPro;

public class NumberDestroyob : MonoBehaviour
{
    [Header("正解の4桁コード")]
    [SerializeField] private string correctCode = "1234";

    [Header("コード表示用のTMPテキスト")]
    [SerializeField] private TextMeshProUGUI codeText;

    private bool playerInRange = false;
    private string currentInput = "";

    private void Start()
    {
        if (codeText != null)
        {
            codeText.gameObject.SetActive(false);
            codeText.text = "";
        }
    }

    private void Update()
    {
        // 範囲内 + 数字キー入力受付
        if (playerInRange)
        {
            // 0～9のキーを検知
            for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    AddDigit((key - KeyCode.Alpha0).ToString());
                }
            }

            // キーボードのテンキー対応
            for (KeyCode key = KeyCode.Keypad0; key <= KeyCode.Keypad9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    AddDigit((key - KeyCode.Keypad0).ToString());
                }
            }

            // バックスペースで削除
            if (Input.GetKeyDown(KeyCode.Backspace) && currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                UpdateText();
            }
        }
    }

    private void AddDigit(string digit)
    {
        if (currentInput.Length >= 4) return;

        currentInput += digit;
        UpdateText();

        if (currentInput.Length == 4)
        {
            CheckCode();
        }
    }

    private void UpdateText()
    {
        if (codeText != null)
            codeText.text = currentInput;
    }

    private void CheckCode()
    {
        if (currentInput == correctCode)
        {
            Destroy(gameObject);
        }
        else
        {
            currentInput = "";
            UpdateText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            codeText.gameObject.SetActive(true);
            currentInput = "";
            UpdateText();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            codeText.gameObject.SetActive(false);
            currentInput = "";
        }
    }
}
