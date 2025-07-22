using DataPersistence;
using Item.Grid;
using Managers;
using Scriptables.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class MachineShop : MonoBehaviour, IPointerClickHandler, IDataPersistence
{
    
    [SerializeField] private BaseItemScriptableObject machine;
    [SerializeField] private int price;
    [SerializeField] private TextMeshProUGUI priceText;
    
    [SerializeField] private bool isCoffeeMachine;
    private bool isFirstMachine;

    private InventoryManager _playerInventory;

    private void Start()
    {
        GameObject playerGrid = GameObject.FindWithTag("InventoryGrid");
        if (playerGrid != null)
        {
            _playerInventory = playerGrid.GetComponent<InventoryManager>();
        }

        if (isCoffeeMachine && isFirstMachine)
        {
            priceText.text = "FREE";
        }
        else
        {
            priceText.text = price.ToString();
        }
    }

    public void LoadData(GameData data)
    {
        if (isCoffeeMachine)
        {
            this.isFirstMachine = data.isFirstMachine;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (isCoffeeMachine)
        {
            data.isFirstMachine = this.isFirstMachine;
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFirstMachine && _playerInventory.HasEmptySlot())
        {
            isFirstMachine = false;
            _playerInventory.AddItem(machine);
            SoundManager.Instance.PlaySFX("Shop_Purchase");
            priceText.text = price.ToString();
            return;
        }
        
        if (GameManager.Instance.CanBuyItem(price) && _playerInventory.HasEmptySlot())
        {
            _playerInventory.AddItem(machine);
            GameManager.Instance.RemoveCoins(price);
            SoundManager.Instance.PlaySFX("Shop_Purchase");
        }
    }
    
}
