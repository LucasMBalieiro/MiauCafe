using Item;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSlot : MonoBehaviour, IDropHandler
{

    private ItemCombiner _itemCombiner;

    private void Awake()
    {
        // Get the ItemCombiner component. It should be on the same GameObject or a parent.
        _itemCombiner = GetComponent<ItemCombiner>();
        if (_itemCombiner == null)
        {
            Debug.LogError("GridSlot requires an ItemCombiner component on itself or a parent!", this);
        }
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        
        var droppedItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        
        if (transform.childCount == 0)
        {
            droppedItem.parentAfterDrag = transform;
        }
        else
        {
            var existingItem = transform.GetChild(0).GetComponent<DraggableItem>();
            
            if (!_itemCombiner.TryCombineItems(existingItem, droppedItem)) 
                droppedItem.ReturnToPreviousPosition();
        }
    }

}
