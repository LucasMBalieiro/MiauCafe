using System;
using Managers;
using Scriptables.Item;
using UnityEditor.Tilemaps;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    [Header("Referencia do objeto pai do grid")]
    [SerializeField] private Transform inventoryGridParent; 
    [SerializeField] private GameObject draggableItemPrefab;
    private GridSlot[] gridSlots;

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        RefreshSlots();
    }

    public void AddItem(BaseItemScriptableObject itemDataToAdd)
    {
        if (itemDataToAdd == null)
        {
            Debug.LogError("Attempted to add a null item to inventory!");
            return;
        }

        foreach (var gridSlot in gridSlots)
        {
            if (gridSlot.transform.childCount == 0) // Find an empty slot
            {
                SpawnItem(itemDataToAdd, gridSlot);
                return; // Item added, exit
            }
        }
        Debug.Log("Inventory full! No available slots to add item.");
    }

    private void SpawnItem(BaseItemScriptableObject itemData, GridSlot gridSlot)
    {
        // 1. Instantiate the prefab (which has the DraggableItem component)
        GameObject spawnedItemGO = Instantiate(draggableItemPrefab, gridSlot.transform);
        
        // 2. Get the DraggableItem component from the spawned GameObject
        DraggableItem draggableItem = spawnedItemGO.GetComponent<DraggableItem>();
        
        if (draggableItem == null)
        {
            Debug.LogError("DraggableItem prefab is missing DraggableItem component!");
            Destroy(spawnedItemGO);
            return;
        }
        
        draggableItem.Initialize(itemData);
    }

    public void RefreshSlots()
    {
        gridSlots = inventoryGridParent.GetComponentsInChildren<GridSlot>();
    }
}
