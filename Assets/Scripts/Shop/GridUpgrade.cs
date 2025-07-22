using DataPersistence;
using Item.Grid;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GridUpgrade : MonoBehaviour, IPointerClickHandler, IDataPersistence
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
    
    private void Start()
    {
        GameObject playerGrid = GameObject.FindWithTag("InventoryGrid");
        if (playerGrid == null) {
            Debug.LogError("GridUpgrade could not find a GameObject with the tag 'InventoryGrid'!");
            return;
        }

        _inventoryManager = playerGrid.GetComponent<InventoryManager>();
        _inventoryParent = playerGrid.transform;
    
        // Safety check for the loaded tier
        if (upgradeTier >= 0 && upgradeTier < upgradePath.Length)
        {
            // Directly build the grid for the tier we currently own
            UpdateInventory(upgradePath[upgradeTier]);
        
            // Populate it with any saved items
            _inventoryManager.InstantiateSavedData();

            // Update the price text to show the cost for the NEXT upgrade
            UpdatePrice();
        }
        else
        {
            Debug.LogError($"Loaded an invalid upgradeTier ({upgradeTier})!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // The tier we WANT to buy is the one after our current one.
        int nextTierIndex = upgradeTier + 1;

        // Check if there is a next tier available to upgrade to.
        if (nextTierIndex < upgradePath.Length)
        {
            // Check if we can afford the price of the NEXT tier.
            if (GameManager.Instance.CanBuyItem(upgradePath[nextTierIndex].price))
            {
                // 1. Pay the price for the NEXT tier.
                GameManager.Instance.RemoveCoins(upgradePath[nextTierIndex].price);
            
                // 2. Build the grid for the NEXT tier.
                UpdateInventory(upgradePath[nextTierIndex]);
            
                // 3. Our new current tier IS the next tier.
                upgradeTier = nextTierIndex;
            
                // 4. Update the price text for the new "next" tier.
                UpdatePrice();
            }
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
    }

    private void UpdatePrice()
    {
        // The price to show is for the tier AFTER our current one.
        int nextTierIndex = upgradeTier + 1;

        if (nextTierIndex < upgradePath.Length)
        {
            priceText.text = upgradePath[nextTierIndex].price.ToString();
        }
        else
        {
            // If there are no more upgrades, show "MAX".
            priceText.text = "MAX";
        }
    }

    public void LoadData(GameData data)
    {
        this.upgradeTier = data.currentGridTier;
    }

    public void SaveData(ref GameData data)
    {
        data.currentGridTier = upgradeTier;
    }
}
