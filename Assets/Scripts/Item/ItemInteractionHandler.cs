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
            if (_itemData.Category == ItemCategory.Machine)
            {
                //É valido adicionar um preço pras máquinas ainda? Se for coloca aqui
                
                if (_machineRuntimeData.CurrentCharges > 0 && InventoryManager.Instance.HasEmptySlot())
                {
                    BaseItemScriptableObject producedItem = _machineRuntimeData.TryProduceItem();
                    
                    if (producedItem != null)
                    {
                        InventoryManager.Instance.AddItem(producedItem);
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