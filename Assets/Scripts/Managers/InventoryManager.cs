using Item;
using UnityEditor.Tilemaps;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    
    [SerializeField] private GridSlot[] gridSlot;
    
    public void AddItem(ItemID itemID)
    {
        foreach (var t in gridSlot)
        {
            if (t.transform.childCount != 0) continue;
            SpawnItem(itemID, t);
            return;
        }
    }

    public void SpawnItem(ItemID itemID, GridSlot gridSlot)
    {
        
    }
}
