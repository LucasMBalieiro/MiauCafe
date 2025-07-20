using System;
using Item.Machine;
using Scriptables.Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Item.General
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
    
        public BaseItemScriptableObject ItemData{ get; private set; }
        
        [Header("Sprite")]
        [SerializeField] private Image _itemImage;
        
        [HideInInspector] public Transform parentAfterDrag;
        [HideInInspector] public Transform previousParent;
        
        private ItemDisplay _itemDisplay;
        private ItemInteractionHandler _itemInteractionHandler;
        
        private MachineRuntimeData _machineRuntimeData;
        private MachineItemDisplay _machineItemDisplay;
        
        //Alex, se quiser usar esses eventos pra fazer o som de combinação, dps posso te explicar como usar
        // (ou pode só ver na internet como faz kkkk)
        public delegate void ItemDroppedEvent(DraggableItem item, Transform newParent);
        public static event ItemDroppedEvent OnItemDropped;

        private void Awake()
        {
            _itemDisplay = GetComponent<ItemDisplay>();
            _itemInteractionHandler = GetComponent<ItemInteractionHandler>();
            
            _machineItemDisplay = GetComponent<MachineItemDisplay>();
            if (_machineItemDisplay != null)
            {
                _machineItemDisplay.SetVisualsActive(false);
            }
        }
        
        public void Initialize(BaseItemScriptableObject newItemData)
        {
            ItemData = newItemData;
            
            if (newItemData.Category == ItemCategory.Machine)
            {
                _machineRuntimeData = GetComponent<MachineRuntimeData>();
                if (_machineRuntimeData == null)
                {
                    _machineRuntimeData = gameObject.AddComponent<MachineRuntimeData>();
                }
                _machineRuntimeData.Initialize(newItemData as MachineScriptableObject);
            }
            else
            {
                if (_machineRuntimeData != null)
                {
                    Destroy(_machineRuntimeData);
                    _machineRuntimeData = null;
                }
            }
            
            _itemDisplay?.SetItem(newItemData);
            
            _itemInteractionHandler.SetItem(newItemData, _machineRuntimeData);
            
            if (_machineItemDisplay != null)
            {
                if (newItemData.Category == ItemCategory.Machine)
                {
                    _machineItemDisplay.Initialize(_machineRuntimeData);
                    _machineItemDisplay.SetVisualsActive(true);
                }
                else
                {
                    _machineItemDisplay.SetVisualsActive(false);
                }
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            //SoundManager.Instance.PlaySFX("Item_Grab");
            parentAfterDrag = transform.parent;
            previousParent = parentAfterDrag; 
            
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            _itemImage.raycastTarget = false;
            
        }
        
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            
        }

        

        public void OnEndDrag(PointerEventData eventData)
        {
            SoundManager.Instance.PlaySFX("Machine_Release");

			transform.SetParent(parentAfterDrag);
            _itemImage.raycastTarget = true;
            

            OnItemDropped?.Invoke(this, parentAfterDrag);
        }
    
        public void ReturnToPreviousPosition()
        {
            transform.SetParent(previousParent);
            parentAfterDrag = previousParent;
            transform.localPosition = Vector3.zero;
            SoundManager.Instance.PlaySFX("MergeError");
        }
    }
}
