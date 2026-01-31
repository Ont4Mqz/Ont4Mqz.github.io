using UnityEngine;
using System.Collections.Generic;

public class BurgerDatabase : MonoBehaviour
{
    [SerializeField] private List<BurgerIngredient> ingredients; // 登録する具材データ
    private Dictionary<string, BurgerIngredient> dict;           // 名前→データ検索用

    private void Awake()
    {
        dict = new Dictionary<string, BurgerIngredient>();
        foreach (var ing in ingredients)
            dict[ing.ingredientName.ToLower()] = ing; // 小文字で統一
    }

    public BurgerIngredient GetIngredient(string name)
    {
        name = name.ToLower();
        if (dict.ContainsKey(name)) return dict[name];
        return null;
    }
}
