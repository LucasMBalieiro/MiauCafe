using System.Linq;
using Item.General;
using Scriptables.Item;
using UnityEngine;

namespace Item.Grid
{
    public class InventoryManager : MonoBehaviour
    {
    
        [Header("Referencia do objeto pai do grid")]
        [SerializeField] private Transform inventoryGridParent; 
        [SerializeField] private GameObject draggableItemPrefab;
        private GridSlot[] gridSlots;
    
        public bool HasEmptySlot()
        {
            return gridSlots.Any(gridSlot => gridSlot.transform.childCount == 0);
        }

        public void AddItem(BaseItemScriptableObject itemDataToAdd)
        {
            if (!HasEmptySlot())
            {
                Debug.Log("Inventory full");
                return;
            }
        
            foreach (var gridSlot in gridSlots)
            {
                if (gridSlot.transform.childCount == 0)
                {
                    SpawnItem(itemDataToAdd, gridSlot);
                    return;
                }
            }
        }

        private void SpawnItem(BaseItemScriptableObject itemData, GridSlot gridSlot)
        {

            GameObject spawnedItem = Instantiate(draggableItemPrefab, gridSlot.transform);
        
            DraggableItem draggableItem = spawnedItem.GetComponent<DraggableItem>();
        
            if (draggableItem == null)
            {
                Debug.LogError("InventoryManager: DraggableItem script == null");
                Destroy(spawnedItem);
                return;
            }
        
            draggableItem.Initialize(itemData);
        }

        public void RefreshSlots()
        {
            gridSlots = inventoryGridParent.GetComponentsInChildren<GridSlot>();
        }
    }
}
