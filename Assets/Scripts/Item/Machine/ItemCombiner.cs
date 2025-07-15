using Item.General;
using Managers;
using Scriptables.Item;
using UnityEngine;
using System.Collections;

namespace Item.Machine
{
    public class ItemCombiner : MonoBehaviour
    {
        float scaleDownTarget = 0.01f;
        float scaleDownDuration = 0.2f;
        float scaleUpDuration = 0.2f;   

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
                    BaseItemScriptableObject nextTierItem = ItemRegistry.GetNextTierItem(existingItemData);
                    SoundManager.Instance.PlaySFX("Item_Merge");

                    if (nextTierItem != null)
                    {
                        //PerformCombination(existingDraggableItem, droppedDraggableItem, nextTierItem);
                        StartCoroutine(PerformCombination(existingDraggableItem, droppedDraggableItem, nextTierItem));
                        return true;
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

        IEnumerator PerformCombination(DraggableItem existingItem, DraggableItem droppedItem, BaseItemScriptableObject newResultItemData)
        {
            Vector3 originalScale = existingItem.transform.localScale;

            // Phase 1: Scale both objects down simultaneously
            // Start both scale down coroutines and wait for them to complete
            StartCoroutine(ScaleObject(existingItem.gameObject, new Vector3(scaleDownTarget, scaleDownTarget, scaleDownTarget), scaleDownDuration));
            StartCoroutine(ScaleObject(droppedItem.gameObject, new Vector3(scaleDownTarget, scaleDownTarget, scaleDownTarget), scaleDownDuration));
            yield return new WaitForSeconds(scaleDownDuration);

            existingItem.Initialize(newResultItemData); 
            Destroy(droppedItem.gameObject);

            // Phase 2: Scale objectToScale1 back up to its original size
            yield return StartCoroutine(ScaleObject(existingItem.gameObject, originalScale, scaleUpDuration));
        }

        IEnumerator ScaleObject(GameObject obj, Vector3 targetScale, float duration)
        {
            if (obj == null)
            {
                Debug.LogWarning("Attempted to scale a null object.");
                yield break; // Exit the coroutine if the object is null
            }

            Vector3 initialScale = obj.transform.localScale;
            float timer = 0f;

            while (timer < duration)
            {
                // Calculate the interpolation factor (0 to 1)
                float t = timer / duration;

                // Smoothly interpolate between the initial and target scale
                obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

                // Increment the timer by the time passed since the last frame
                timer += Time.deltaTime;

                // Wait for the next frame
                yield return null;
            }

            // Ensure the object reaches the exact target scale at the end
            obj.transform.localScale = targetScale;
        }
    }
}