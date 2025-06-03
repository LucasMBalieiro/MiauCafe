using Scriptables.Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class MachineShop : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private BaseItemScriptableObject machine;
    [SerializeField] private int price;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (CoinController.Instance.CanBuyItem(price) && InventoryManager.Instance.HasEmptySlot())
        {
            InventoryManager.Instance.AddItem(machine);
            CoinController.Instance.RemoveCoins(price);
        }
    }
}
