using UnityEngine;

[CreateAssetMenu(menuName = "Burger/Ingredient")]
public class BurgerIngredient : ScriptableObject
{
    public string ingredientName;      // 具材名
    public GameObject prefab;          // 対応Prefab
}
