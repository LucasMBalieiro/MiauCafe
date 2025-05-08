using UnityEngine;
using UnityEngine.EventSystems;

public class GridSlot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        
        DraggableItem droppedItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        
        if (transform.childCount == 0)
        {
            droppedItem.parentAfterDrag = transform;
        }
        else
        {
            DraggableItem existingItem = transform.GetChild(0).GetComponent<DraggableItem>();
            
            if (!existingItem.HandleCombination(droppedItem))
            {
                droppedItem.ReturnToPreviousPosition();
            }
        }
    }
    
    
    private void HandleItemCombination(DraggableItem existingItem, DraggableItem droppedItem)
    {
        // Example combination logic:
        // 1. Increase level of existing item
        existingItem.itemID.tier++;
        
        // 2. Destroy the dropped item
        Destroy(droppedItem.gameObject);
        
        // 3. Update visuals if needed
        // existingItem.UpdateVisuals();
        
        Debug.Log($"New item level: {existingItem.itemID.GetCompositeID()}");
    }
}
