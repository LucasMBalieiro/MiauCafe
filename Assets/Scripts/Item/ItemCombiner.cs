using Managers;
using Scriptables.Item;
using UnityEngine;

namespace Item
{
    public class ItemCombiner : MonoBehaviour
    {

        public bool TryCombineItems(DraggableItem existingDraggableItem, DraggableItem droppedDraggableItem)
        {
            BaseItemScriptableObject existingItemData = existingDraggableItem.ItemData;
            BaseItemScriptableObject droppedItemData = droppedDraggableItem.ItemData;
            
            
            if (existingItemData.Category == droppedItemData.Category && existingItemData.tier == droppedItemData.tier)
            {
                bool typesMatch = false;
                
                if (existingItemData.Category == ItemCategory.Ingredient &&
                    existingItemData is IngredientScriptableObject ing1 &&
                    droppedItemData is IngredientScriptableObject ing2 &&
                    ing1.ingredientType == ing2.ingredientType)
                {
                    typesMatch = true;
                }
                else if (existingItemData.Category == ItemCategory.Machine &&
                         existingItemData is MachineScriptableObject mach1 &&
                         droppedItemData is MachineScriptableObject mach2 &&
                         mach1.machineType == mach2.machineType)
                {
                    typesMatch = true;
                }

                if (typesMatch)
                {
                    BaseItemScriptableObject nextTierItem = ItemRegistry.Instance.GetNextTierItem(existingItemData);
                    SoundManager.Instance.PlaySFX("Item_Merge");

                    if (nextTierItem != null)
                    {
                        return PerformCombination(existingDraggableItem, droppedDraggableItem, nextTierItem);
                    }
                }
            }

            /* Se tiver o petit gateau la 
            BaseItemScriptableObject specialCombinationResult = ItemRegistry.Instance.GetCombinationResult(existingItemData, droppedItemData);
            if (specialCombinationResult != null)
            {
                return PerformCombination(existingDraggableItem, droppedDraggableItem, specialCombinationResult);
            }
            */

            return false;
        }

        private static bool PerformCombination(DraggableItem existingItem, DraggableItem droppedItem, BaseItemScriptableObject newResultItemData)
        {
            existingItem.Initialize(newResultItemData); 
            Destroy(droppedItem.gameObject);

            return true;
        }
    }
}