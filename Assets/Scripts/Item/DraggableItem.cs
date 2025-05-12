using Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Managers;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public ItemID itemID;
    
    [Header("Sprite")]
    [SerializeField] private Image itemImage;
    
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform previousParent;
    

    private void Start()
    {
        UpdateVisuals();
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
    
    public bool HandleCombination(DraggableItem otherItem)
    {
        //TODO: Precisamos tratar usando IsEqualTier e IsEqualType no futuro
        if (!itemID.IsEqual(otherItem.itemID)) 
            return false;

        return SpriteManager.Instance.TierExists(itemID) && CombineItems(otherItem);
        
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
        itemImage.sprite = SpriteManager.Instance.GetSpriteForItem(itemID);
    }
    
    public void ReturnToPreviousPosition()
    {
        transform.SetParent(previousParent);
        parentAfterDrag = previousParent;
        transform.localPosition = Vector3.zero;
    }
}
