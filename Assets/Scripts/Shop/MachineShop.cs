using Scriptables.Item;
using UnityEngine;
using UnityEngine.EventSystems;

public class MachineShop : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private BaseItemScriptableObject machine;
    [SerializeField] private int price;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (CoinController.Instance.BuyItem(price))
        {
            InventoryManager.Instance.AddItem(machine);
        }
    }
}
