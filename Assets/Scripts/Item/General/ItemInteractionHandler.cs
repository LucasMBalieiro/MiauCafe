using Item.Grid;
using Item.Machine;
using Managers;
using Scriptables.Item;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Item.General
{
    public class ItemInteractionHandler : MonoBehaviour, IPointerClickHandler
    {
        private BaseItemScriptableObject _itemData;
        private MachineRuntimeData _machineRuntimeData;
        private InventoryManager _inventoryManager;

        private void Awake()
        {
            GameObject playerGrid = GameObject.FindWithTag("InventoryGrid");
            if (playerGrid != null)
            {
                _inventoryManager = playerGrid.GetComponent<InventoryManager>();
            }
        }

        public void SetItem(BaseItemScriptableObject itemData, MachineRuntimeData machineRuntimeData = null)
        {
            _itemData = itemData;
            _machineRuntimeData = machineRuntimeData;
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_itemData.Category == ItemCategory.Machine && _itemData is MachineScriptableObject machine)
            {
                
                
                if (_machineRuntimeData.CurrentCharges > 0 && _inventoryManager.HasEmptySlot() && GameManager.Instance.CanBuyItem(machine.cost))
                {
                    BaseItemScriptableObject producedItem = _machineRuntimeData.TryProduceItem();
                    if (producedItem != null)
                    {
                        SoundManager.Instance.PlaySFX("GenerateIngredient");
                        _inventoryManager.AddItem(producedItem);
                        GameManager.Instance.RemoveCoins(machine.cost);
                    }
                }
                else
                {
                    SoundManager.Instance.PlaySFX("Machine_Grab");
                }
            }
            else
            {
                SoundManager.Instance.PlaySFX("Item_Grab");
            }
            
        }
    }
}