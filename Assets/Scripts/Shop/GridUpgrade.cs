using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GridUpgrade : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private Transform inventoryParent;

    
    [System.Serializable]public class UpgradePath
    {
        public int numSlots;
        public int collumCount;
        public int price;
    }
    
    [SerializeField] private UpgradePath[] upgradePath;
    [SerializeField] private int upgradeTier;
    
    private void Awake()
    {
        if (inventoryParent.childCount == 0 && upgradeTier == 0)
        {
            UpdateInventory(upgradePath[upgradeTier]);
        }
        else
        {
            Debug.LogError("GridUpgrade: atributos errados/faltando");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (upgradeTier < upgradePath.Length && CoinController.Instance.CanBuyItem(upgradePath[upgradeTier].price))
        {
            CoinController.Instance.RemoveCoins(upgradePath[upgradeTier].price);
            UpdateInventory(upgradePath[upgradeTier]);
        }
    }
    
    private void UpdateInventory(UpgradePath upgradePath)
    {
        inventoryParent.GetComponent<GridLayoutGroup>().constraintCount = upgradePath.collumCount;

        int newSlots = upgradePath.numSlots - inventoryParent.childCount;
        
        for(int i = 0; i < newSlots; i++)
        {
            GameObject newItem = Instantiate(gridPrefab, inventoryParent);
        }
        InventoryManager.Instance.RefreshSlots();
        upgradeTier++;
    }
}
