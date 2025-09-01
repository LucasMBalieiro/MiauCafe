using Item.General;
using Scriptables.Item;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DropItemTutorial : MonoBehaviour, IDropHandler
{
    
    public UnityEvent onDropped = new UnityEvent();
    public UnityEvent finalOrder = new UnityEvent();
    private int dropCount = 0;
    
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem droppedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (droppedItem.ItemData.Category == ItemCategory.Ingredient)
        {
            onDropped.Invoke();
            dropCount++;
        }

        if (dropCount == 3)
        {
            finalOrder.Invoke();
        }
    }
}
