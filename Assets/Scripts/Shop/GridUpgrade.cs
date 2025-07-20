using Item.Grid;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GridUpgrade : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject gridPrefab;
    
    private InventoryManager _inventoryManager;
    private Transform _inventoryParent;
    
    [System.Serializable]public class UpgradePath
    {
        public int numSlots;
        public int columCount;
        public int price;
    }
    
    [SerializeField] private UpgradePath[] upgradePath;
    [SerializeField] private int upgradeTier;
    
    [SerializeField] private TextMeshProUGUI priceText;
    
    private void Awake()
    {
        GameObject playerGrid = GameObject.FindWithTag("InventoryGrid");
        if (playerGrid != null)
        {
            _inventoryManager = playerGrid.GetComponent<InventoryManager>();
            _inventoryParent = playerGrid.transform;
            
            if (_inventoryParent.childCount == 0 && upgradeTier == 0)
            {
                UpdateInventory(upgradePath[upgradeTier]);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (upgradeTier < upgradePath.Length && GameManager.Instance.CanBuyItem(upgradePath[upgradeTier].price))
        {
            GameManager.Instance.RemoveCoins(upgradePath[upgradeTier].price);
            UpdateInventory(upgradePath[upgradeTier]);
            //TODO: inserir som próprio
        }
    }
    
    private void UpdateInventory(UpgradePath upgradePath)
    {
        _inventoryParent.GetComponent<GridLayoutGroup>().constraintCount = upgradePath.columCount;

        int newSlots = upgradePath.numSlots - _inventoryParent.childCount;
        
        for(int i = 0; i < newSlots; i++)
        {
            GameObject newItem = Instantiate(gridPrefab, _inventoryParent);
        }
        _inventoryManager.RefreshSlots();
        upgradeTier++;
        UpdatePrice();
    }

    private void UpdatePrice()
    {
        if (upgradeTier < upgradePath.Length)
        {
            priceText.text = upgradePath[upgradeTier].price.ToString();
        }
        else
        {
            priceText.text = "MAX";
        }
    }
}
