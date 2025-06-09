using Scriptables.Item;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Item
{
    public class ItemInteractionHandler : MonoBehaviour, IPointerClickHandler
    {
        private BaseItemScriptableObject _itemData;
        private MachineRuntimeData _machineRuntimeData;

        public void SetItem(BaseItemScriptableObject itemData, MachineRuntimeData machineRuntimeData = null)
        {
            _itemData = itemData;
            _machineRuntimeData = machineRuntimeData;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_itemData.Category == ItemCategory.Machine && _itemData is MachineScriptableObject machine)
            {
                if (_machineRuntimeData.CurrentCharges > 0 && InventoryManager.Instance.HasEmptySlot() && CoinController.Instance.CanBuyItem(machine.cost))
                {
                    BaseItemScriptableObject producedItem = _machineRuntimeData.TryProduceItem();

                    if (producedItem != null)
                    {
                        InventoryManager.Instance.AddItem(producedItem);
                        CoinController.Instance.RemoveCoins(machine.cost);
                    }
                    else
                    {
                        Debug.LogWarning("Maquina não possui SO equivalente para ingredientes");
                    }
                }
            }
            else
            {
                //Se for colocar aquele negocio de ingrediente brilhar quando clickar
            }
        }
    }
}