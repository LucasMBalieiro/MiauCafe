using UnityEngine;
using UnityEngine.EventSystems;

public class GridSlot : MonoBehaviour, IDropHandler
{

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
            
            if (!existingItem.HandleCombination(droppedItem))
            {
                droppedItem.ReturnToPreviousPosition();
            }
        }
    }

}
