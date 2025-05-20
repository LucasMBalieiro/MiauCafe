using System;
using Item;
using Managers;
using UnityEditor.Tilemaps;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    [Header("Referencia do objeto pai do grid")]
    [SerializeField] private Transform inventory;
    [SerializeField] private GameObject ingredientePrefab;
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

    public void AddItem(ItemID itemID)
    {
        foreach (var gridSlot in gridSlots)
        {
            if (gridSlot.transform.childCount != 0) continue;
            SpawnItem(itemID, gridSlot);
            return;
        }
        //TODO: colocar um sonzinho se o grid acabou?
        Debug.Log("Acabou os slots");
    }

    private void SpawnItem(ItemID itemID, GridSlot gridSlot)
    {
        int id = (int) itemID.type - 10;
        int randomTier = DropRates.Instance.CalculateTierDrop(itemID.tier);
        
        GameObject spawnedItem = Instantiate(ingredientePrefab, gridSlot.transform);
        DraggableItem draggableItem = spawnedItem.GetComponent<DraggableItem>();
        
        draggableItem.Initialize(new ItemID((ItemType)id, randomTier));
    }

    private void RefreshSlots()
    {
        gridSlots = inventory.GetComponentsInChildren<GridSlot>();
    }
}
