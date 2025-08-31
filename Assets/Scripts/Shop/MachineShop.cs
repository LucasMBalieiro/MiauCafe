using System;
using System.Collections;
using DataPersistence;
using Item.Grid;
using Managers;
using Scriptables.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class MachineShop : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDataPersistence
{
    
    [SerializeField] private BaseItemScriptableObject machine;
    [SerializeField] private int[] price;
    
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private GameObject hoverTip;
    
    [SerializeField] private MachineType machineType;
    
    private GameManager gameManager;
    private int currentQuantity;
    private int maxQuantity;
    private bool isFirstMachine;
    
    private int priceListQuantity;
    
    private Coroutine _hoverCoroutine;

    private InventoryManager _playerInventory;

    private void Start()
    {
        GameObject playerGrid = GameObject.FindWithTag("InventoryGrid");
        if (playerGrid != null)
        {
            _playerInventory = playerGrid.GetComponent<InventoryManager>();
        }
        
        gameManager = GameManager.Instance;
        
        SetQuantity();
        SetPrice();
    }

    private void SetPrice()
    {
        if (machineType == MachineType.MaquinaCafe && isFirstMachine)
        {
            priceText.text = "FREE";
        }
        else
        {
            priceText.text = price[priceListQuantity].ToString();
        }
    }

    private void SetQuantity()
    {
        currentQuantity = gameManager.GetCurrentMachineQuantity(machineType);
        maxQuantity = gameManager.GetMaxMachineQuantity(machineType);

        if (currentQuantity < price.Length)
        {
            priceListQuantity = currentQuantity;
        }
        
        quantityText.text = currentQuantity.ToString() + "/" + maxQuantity.ToString();
    }

    public void LoadData(GameData data)
    {
        if (machineType == MachineType.MaquinaCafe)
        {
            this.isFirstMachine = data.isFirstMachine;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (machineType == MachineType.MaquinaCafe)
        {
            data.isFirstMachine = this.isFirstMachine;
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFirstMachine && _playerInventory.HasEmptySlot())
        {
            isFirstMachine = false;
            gameManager.AddMachineQuantity(machineType);
            
            _playerInventory.AddItem(machine);
            SoundManager.Instance.PlaySFX("Shop_Purchase");
            
            SetQuantity();
            SetPrice();
            return;
        }

        if (currentQuantity < maxQuantity && gameManager.CanBuyItem(price[priceListQuantity]) && _playerInventory.HasEmptySlot())
        {
            gameManager.AddMachineQuantity(machineType);
            _playerInventory.AddItem(machine);
            gameManager.RemoveCoins(price[priceListQuantity]);
            SoundManager.Instance.PlaySFX("Shop_Purchase");
            SetQuantity();
            SetPrice();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hoverCoroutine = StartCoroutine(HoverCoroutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_hoverCoroutine != null)
        {
            StopCoroutine(_hoverCoroutine);
        }
        
        hoverTip.SetActive(false);
    }
    
    private IEnumerator HoverCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        hoverTip.SetActive(true);
        
    }
    
}
