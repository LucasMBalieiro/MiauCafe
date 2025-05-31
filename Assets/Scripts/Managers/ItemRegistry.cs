using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Scriptables.Item;

namespace Managers
{
    [CreateAssetMenu(fileName = "ItemRegistry", menuName = "Items/Item Registry")]
    public class ItemRegistry : ScriptableObject
    {
        private static ItemRegistry _instance;
        public static ItemRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ItemRegistry>("ItemRegistry"); //esse metodo é mt mais pica
                    if (_instance == null)
                    {
                        Debug.LogError("ItemRegistry asset not found! Create one under a 'Resources' folder and name it 'ItemRegistry'.");
                    }
                }
                return _instance;
            }
        }
        
        [Header("Todos os itens: ")]
        public List<BaseItemScriptableObject> allItems;
    
        // (ItemCategory, Type (enum int), Tier)
        private Dictionary<(ItemCategory, int, int), BaseItemScriptableObject> _itemLookup;
        
        private void OnEnable()
        {
            InitializeLookup();
        }
        
        private void OnValidate()
        {
        #if UNITY_EDITOR
            // Only re-initialize in editor to catch changes to the 'allItems' list
            InitializeLookup();
        #endif
        }
        
        private void InitializeLookup()
        {
            if (allItems == null) return;

            _itemLookup = new Dictionary<(ItemCategory, int, int), BaseItemScriptableObject>();
            foreach (var item in allItems)
            {
                if (item == null)
                {
                    continue;
                }

                int typeId = -1;
                
                if (_itemLookup.ContainsKey((item.Category, typeId, item.tier)))
                {
                    Debug.LogWarning($"{item.name} esta duplicado");
                    break;
                }

                if (item.Category == ItemCategory.Ingredient && item is IngredientScriptableObject ingredient)
                {
                    typeId = (int)ingredient.ingredientType;
                }
                else if (item.Category == ItemCategory.Machine && item is MachineScriptableObject machine)
                {
                    typeId = (int)machine.machineType;
                }
                else
                {
                    Debug.LogWarning($"{item.name} foi inserido errado, sem categoria");
                    break;
                }
                _itemLookup[(item.Category, typeId, item.tier)] = item;
            }
        }
        
        public BaseItemScriptableObject GetNextTierItem(BaseItemScriptableObject currentItem)
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
        
        
        public BaseItemScriptableObject GetItem(ItemCategory category, int typeId, int tier)
        {
            _itemLookup.TryGetValue((category, typeId, tier), out BaseItemScriptableObject item);
            return item;
        }
        public IngredientScriptableObject GetIngredient(IngredientType type, int tier)
        {
            return GetItem(ItemCategory.Ingredient, (int)type, tier) as IngredientScriptableObject;
        }
        public MachineScriptableObject GetMachine(MachineType type, int tier)
        {
            return GetItem(ItemCategory.Machine, (int)type, tier) as MachineScriptableObject;
        }
        
        
        public BaseItemScriptableObject GetCombinationResult(BaseItemScriptableObject item1, BaseItemScriptableObject item2)
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
