using UnityEngine;

namespace Scriptables.Item
{
    [CreateAssetMenu(fileName = "NewIngredient", menuName = "Items/Ingredient")]
    public class IngredientScriptableObject : BaseItemScriptableObject
    {
        public IngredientType ingredientType;
        public int cost;

        public override ItemCategory Category => ItemCategory.Ingredient;
        
        //vamo colocar um int valor em cada ingrediente? que ai da pra puxar
        //o lucro dos pedidos direto do somatorio dos ingredientes
    }
}
