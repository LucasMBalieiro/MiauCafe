using Item.Grid;
using Managers;
using Scriptables.Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class MachineShop : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private BaseItemScriptableObject machine;
    [SerializeField] private int price;

    private InventoryManager _playerInventory;

    private void Awake()
    {
        GameObject playerGrid = GameObject.FindWithTag("InventoryGrid");
        if (playerGrid != null)
        {
            _playerInventory = playerGrid.GetComponent<InventoryManager>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.CanBuyItem(price) && _playerInventory.HasEmptySlot())
        {
            _playerInventory.AddItem(machine);
            GameManager.Instance.RemoveCoins(price);
        }
    }
}
