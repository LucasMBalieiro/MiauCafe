using System;
using System.Collections.Generic;
using System.Linq;
using DataPersistence;
using Item.General;
using Managers;
using Scriptables.Item;
using UnityEngine;

namespace Item.Grid
{
    public class InventoryManager : MonoBehaviour, IDataPersistence
    {
        [Header("Referencia do objeto pai do grid")]
        [SerializeField] private Transform inventoryGridParent; 
        [SerializeField] private GameObject draggableItemPrefab;
        private GridSlot[] gridSlots;
        
        private class SavedInventoryItem
        {
            public BaseItemScriptableObject ItemData;
            public int Position;
        }
        
        private List<SavedInventoryItem> _savedInventory = new List<SavedInventoryItem>();

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

        public void InstantiateSavedData()
        {
            ClearAllSlots(); 
            
            if (_savedInventory.Count > 0)
            {
                foreach (SavedInventoryItem savedItem in _savedInventory)
                {
                    GridSlot targetSlot = gridSlots[savedItem.Position];
                    SpawnItem(savedItem.ItemData, targetSlot);
                }
            }
        }
        
        private void ClearAllSlots()
        {
            foreach (var gridSlot in gridSlots)
            {
                foreach (Transform child in gridSlot.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void LoadData(GameData data)
        {
            _savedInventory.Clear();

            for (int i = 0; i < data.inventoryItemTypeIDs.Count; i++)
            {
                ItemCategory category = data.inventoryItemCategories[i];
                int typeID = data.inventoryItemTypeIDs[i];
                int tier = data.inventoryItemTiers[i];
                int positionIndex = data.inventoryItemPositions[i];

                BaseItemScriptableObject itemToSpawn = ItemRegistry.GetItem(category, typeID, tier);
    
                if (itemToSpawn != null)
                {
                    _savedInventory.Add(new SavedInventoryItem
                    {
                        ItemData = itemToSpawn,
                        Position = positionIndex
                    });
                }
            }
        }
        
        public void SaveData(ref GameData data)
        {
            data.inventoryItemCategories.Clear();
            data.inventoryItemTypeIDs.Clear();
            data.inventoryItemTiers.Clear();
            data.inventoryItemPositions.Clear();

            for (int i = 0; i < gridSlots.Length; i++)
            {
                if (gridSlots[i].transform.childCount > 0)
                {
                    DraggableItem itemInSlot = gridSlots[i].transform.GetChild(0).GetComponent<DraggableItem>();
                    BaseItemScriptableObject itemData = itemInSlot.ItemData;

                    if (itemData is MachineScriptableObject machine)
                    {
                        data.inventoryItemCategories.Add(ItemCategory.Machine);
                        data.inventoryItemTypeIDs.Add((int)machine.machineType);
                        data.inventoryItemTiers.Add(machine.tier);
                        data.inventoryItemPositions.Add(i);
                    }
                }
            }
        }
    }
}