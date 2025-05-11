using Item;
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

    private void Awake()
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
        //TODO: Precisamos tratar usando IsEqualTier e IsEqualType no futuro
        if (!itemID.IsEqual(otherItem.itemID)) 
            return false;

        if (spriteHandler.TierExists(itemID.type, itemID.tier + 1)) 
            return CombineItems(otherItem);
        
        Debug.Log($"Nao encontrou proximo. --- Type: {itemID.type} Tier: {itemID.tier} ---");
        return false;
    }
    
    
    private bool CombineItems(DraggableItem otherItem)
    {
        itemID.tier++;
        UpdateVisuals();
        
        Destroy(otherItem.gameObject);

        return true;
    }

    private void UpdateVisuals()
    {
        if (spriteHandler != null)
        {
            image.sprite = spriteHandler.GetSpriteForItem(itemID);
        }
    }
    
    public void ReturnToPreviousPosition()
    {
        transform.SetParent(previousParent);
        parentAfterDrag = previousParent;
        transform.localPosition = Vector3.zero;
    }
}
