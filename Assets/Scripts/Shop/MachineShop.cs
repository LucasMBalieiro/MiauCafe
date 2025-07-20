using Item.Grid;
using Managers;
using Scriptables.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class MachineShop : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private BaseItemScriptableObject machine;
    [SerializeField] private int price;
    [SerializeField] private TextMeshProUGUI priceText;

    private InventoryManager _playerInventory;

    private void Awake()
    {
        GameObject playerGrid = GameObject.FindWithTag("InventoryGrid");
        if (playerGrid != null)
        {
            _playerInventory = playerGrid.GetComponent<InventoryManager>();
        }
        
        priceText.text = price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.CanBuyItem(price) && _playerInventory.HasEmptySlot())
        {
            _playerInventory.AddItem(machine);
            GameManager.Instance.RemoveCoins(price);
            SoundManager.Instance.PlaySFX("Shop_Purchase");
        }
    }
    
}
