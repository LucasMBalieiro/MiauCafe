using Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Managers;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    
    public ItemID itemID;
    
    [Header("Sprite")]
    [SerializeField] private Image itemImage;
    
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform previousParent;
    
    //Som do merge
    [Header("mergeAudio")]
    [SerializeField] private AudioClip mergeSound;
    private AudioSource audioSource;
    private void Start()
    {
        UpdateVisuals();
        // Configura o AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0; // 2D
        }

    }

    public void Initialize(ItemID newItemID)
    {
        itemID = newItemID;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemID.IsMachine() && CoinController.Instance.BuyItem(itemID))
        {
            InventoryManager.Instance.AddItem(itemID);
        }
        else
        {
            Debug.Log("Sem dinheiro || não é maquina");
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
    
    public bool HandleCombination(DraggableItem otherItem)
    {
        //TODO: Precisamos tratar usando IsEqualTier e IsEqualType no futuro
        if (!itemID.IsEqual(otherItem.itemID)) 
            return false;

        return SpriteDictionary.Instance.TierExists(itemID) && CombineItems(otherItem);
        
    }
    
    
    private bool CombineItems(DraggableItem otherItem)
    {
        itemID.tier++;
        UpdateVisuals();
        // Toca o som de merge:
        if(mergeSound != null) {
            //caso seja interessante, variar um pouco o mesmo som:
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Variação de 10%
            audioSource.PlayOneShot(mergeSound);

        }
        
        Destroy(otherItem.gameObject);

        return true;
    }

    private void UpdateVisuals()
    {
        itemImage.sprite = SpriteDictionary.Instance.GetSpriteForItem(itemID);
    }
    
    public void ReturnToPreviousPosition()
    {
        transform.SetParent(previousParent);
        parentAfterDrag = previousParent;
        transform.localPosition = Vector3.zero;
    }


}
