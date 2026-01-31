using UnityEngine;
using TMPro;

public class TypingManager : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;     // 表示する単語
    [SerializeField] private BurgerDatabase database;  // データ参照
    [SerializeField] private BurgerSpawner spawner;    // スポーン担当

    private string targetWord;                         // 今の単語
    private string currentInput = "";                  // 入力内容

    private void Start()
    {
        PickNewIngredient();                           // 最初の単語をセット
    }

    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')                             // バックスペース
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput[..^1];
            }
            else if (c != '\n' && c != '\r')
            {
                currentInput += c;                     // 入力追加
            }
        }

        if (currentInput.ToLower() == targetWord.ToLower()) // 一致判定
        {
            SpawnIngredient();
            PickNewIngredient();
        }
    }

    private void PickNewIngredient()                   // 次の具材を選ぶ
    {
        var all = Resources.LoadAll<BurgerIngredient>(""); 
        var rnd = all[Random.Range(0, all.Length)];

        targetWord = rnd.ingredientName;
        displayText.text = targetWord;
        currentInput = "";
    }

    private void SpawnIngredient()                     // 具材生成
    {
        var ing = database.GetIngredient(targetWord);
        if (ing != null) spawner.Spawn(ing.prefab);
    }
}
