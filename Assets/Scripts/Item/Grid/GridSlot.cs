using Item.General;
using Item.Machine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Item.Grid
{
    public class GridSlot : MonoBehaviour, IDropHandler
    {

        private ItemCombiner _itemCombiner;

        private void Awake()
        {
            _itemCombiner = GetComponent<ItemCombiner>();
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
                {
                    existingItem.SwitchParent(droppedItem.previousParent);
                    droppedItem.parentAfterDrag = transform;
                }
            }
        }

    }
}
