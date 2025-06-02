using Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Managers;
using Scriptables.Item;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    
    public BaseItemScriptableObject itemData;
    
    [Header("Sprite")]
    [SerializeField] private Image itemImage;
    
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform previousParent;
    
    
    private void Start()
    {
        if (itemData != null) UpdateVisuals();
    }

    public void Initialize(BaseItemScriptableObject newItemData)
    {
        if (newItemData == null) return;
        
        itemData = newItemData;
        UpdateVisuals();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData.Category == ItemCategory.Machine)
        {
            MachineScriptableObject machineItem = itemData as MachineScriptableObject;
            
            if (machineItem != null && CoinController.Instance.BuyItem(machineItem.cost))
            {
                int tier = DropRates.Instance.CalculateTierDrop(machineItem.tier);
                BaseItemScriptableObject producedItem = ItemRegistry.Instance.GetIngredient(machineItem.producesIngredientType, tier);
                InventoryManager.Instance.AddItem(producedItem);
            }
            else
            {
                Debug.Log("Sem dinheiro para comprar máquina!");
            }
        }
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        previousParent = parentAfterDrag; 
        
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        itemImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        itemImage.raycastTarget = true;
    }
    
    //ver se da pra simplificar
    public bool HandleCombination(DraggableItem otherItem)
    {
        if (itemData.Category == otherItem.itemData.Category && itemData.tier == otherItem.itemData.tier)
        {
            bool typesMatch = false;
            if (itemData.Category == ItemCategory.Ingredient && 
                itemData is IngredientScriptableObject ing1 && 
                otherItem.itemData is IngredientScriptableObject ing2 && 
                ing1.ingredientType == ing2.ingredientType)
            {
                typesMatch = true;
            }
            else if (itemData.Category == ItemCategory.Machine && 
                     itemData is MachineScriptableObject mach1 && 
                     otherItem.itemData is MachineScriptableObject mach2 && 
                     mach1.machineType == mach2.machineType)
            {
                typesMatch = true;
            }

            if (typesMatch)
            {
                BaseItemScriptableObject nextTierItem = ItemRegistry.Instance.GetNextTierItem(itemData);

                if (nextTierItem != null)
                {
                    return CombineItems(otherItem, nextTierItem);
                }
            }
        }
        
        /* Se a gente tiver tiers speciais ta ai
        BaseItemScriptableObject specialCombinationResult = ItemRegistry.Instance.GetCombinationResult(itemData, otherItem.itemData);
        if (specialCombinationResult != null)
        {
            return CombineItems(otherItem, specialCombinationResult);
        }
        */
        return false;
    }
    
    
    private bool CombineItems(DraggableItem otherItem, BaseItemScriptableObject newResultItemData)
    {
        itemData = newResultItemData; 
        UpdateVisuals();
        Destroy(otherItem.gameObject);

        return true;
    }

    private void UpdateVisuals()
    {
        itemImage.sprite = itemData.sprite; 
    }
    
    public void ReturnToPreviousPosition()
    {
        transform.SetParent(previousParent);
        parentAfterDrag = previousParent;
        transform.localPosition = Vector3.zero;
    }


}
