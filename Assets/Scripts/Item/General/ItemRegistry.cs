using System.Collections.Generic;
using Scriptables.Item;
using UnityEngine;

namespace Item.General
{
    public static class ItemRegistry
    {
        // (ItemCategory, Type (enum int), Tier)
        private static Dictionary<(ItemCategory, int, int), BaseItemScriptableObject> _itemLookup;
        
        public static void Initialize(List<BaseItemScriptableObject> initialItems)
        {
            _itemLookup = new Dictionary<(ItemCategory, int, int), BaseItemScriptableObject>();
            foreach (var item in initialItems)
            {
                int typeId = -1;

                if (item.Category == ItemCategory.Ingredient && item is IngredientScriptableObject ingredient)
                {
                    typeId = (int)ingredient.ingredientType;
                }
                else if (item.Category == ItemCategory.Machine && item is MachineScriptableObject machine)
                {
                    typeId = (int)machine.machineType;
                }

                var key = (item.Category, typeId, item.tier);

                if (!_itemLookup.TryAdd(key, item))
                {
                    Debug.LogError($"ItemRegistry: Item {key} - DUPLICADO -");
                }
            }
            Debug.Log($"ItemRegistry initialized: Count = {_itemLookup.Count} items");
        }
        
        public static BaseItemScriptableObject GetNextTierItem(BaseItemScriptableObject currentItem)
        {
            if (currentItem == null) return null;

            int nextTier = currentItem.tier + 1;
            int typeId = -1;

            if (currentItem.Category == ItemCategory.Ingredient && currentItem is IngredientScriptableObject ingredient)
            {
                typeId = (int)ingredient.ingredientType;
            }
            else if (currentItem.Category == ItemCategory.Machine && currentItem is MachineScriptableObject machine)
            {
                typeId = (int)machine.machineType;
            }
            
            return _itemLookup.TryGetValue((currentItem.Category, typeId, nextTier), out BaseItemScriptableObject nextItem) ? nextItem : null;
        }
        
        
        public static BaseItemScriptableObject GetItem(ItemCategory category, int typeId, int tier)
        {
            _itemLookup.TryGetValue((category, typeId, tier), out BaseItemScriptableObject item);
            return item;
        }
        public static IngredientScriptableObject GetIngredient(IngredientType type, int tier)
        {
            return GetItem(ItemCategory.Ingredient, (int)type, tier) as IngredientScriptableObject;
        }
        public static MachineScriptableObject GetMachine(MachineType type, int tier)
        {
            return GetItem(ItemCategory.Machine, (int)type, tier) as MachineScriptableObject;
        }
        
        
        public static BaseItemScriptableObject GetCombinationResult(BaseItemScriptableObject item1, BaseItemScriptableObject item2)
        {
            //Insanidades do Gemini, mas acho que vai ser útil no futuro
            if (item1.Category == ItemCategory.Ingredient && item2.Category == ItemCategory.Ingredient &&
                item1.tier == 4 && item2.tier == 4)
            {
                IngredientScriptableObject ing1 = item1 as IngredientScriptableObject;
                IngredientScriptableObject ing2 = item2 as IngredientScriptableObject;

 
                if ((ing1.ingredientType == IngredientType.Pao && ing2.ingredientType == IngredientType.Bolo) ||
                    (ing1.ingredientType == IngredientType.Bolo && ing2.ingredientType == IngredientType.Pao))
                {
                    return GetIngredient(IngredientType.Cafe, 1);
                }
            }
            return null;
        }
    }
}
