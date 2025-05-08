using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [Header("Identificador")]
    public ItemID itemID;
    
    [Header("Sprite")]
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform previousParent;
    
    public SpriteHandler spriteHandler;

    public void Awake()
    {
        UpdateVisuals();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        previousParent = parentAfterDrag; 
        
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
    
    public bool HandleCombination(DraggableItem otherItem)
    {
        // Check if items are the same type and tier
        if (!itemID.IsEqual(otherItem.itemID)) 
            return false;

        // Check if next tier exists in SpriteHandler
        if (!CanUpgradeToTier(itemID.tier + 1))
        {
            Debug.Log($"Max tier reached for {itemID.type} (Current: {itemID.tier})");
            return false;
        }

        // Proceed with combination
        return CombineItems(otherItem);
    }
    
    private bool CanUpgradeToTier(int targetTier)
    {
        if (spriteHandler == null)
        {
            Debug.LogWarning("SpriteHandler not assigned!");
            return false;
        }

        return spriteHandler.TierExists(itemID.type, targetTier);
    }
    
    private bool CombineItems(DraggableItem otherItem)
    {
        itemID.tier++;
        UpdateVisuals();
        
        Destroy(otherItem.gameObject);
        
        Debug.Log($"New item level: {itemID.GetCompositeID()}");

        return true;
    }

    private void UpdateVisuals()
    {
        if (spriteHandler != null)
        {
            image.sprite = spriteHandler.GetSpriteForItem(itemID.type, itemID.tier);
        }
    }
    
    public void ReturnToPreviousPosition()
    {
        transform.SetParent(previousParent);
        parentAfterDrag = previousParent;
        transform.localPosition = Vector3.zero;
    }
}
