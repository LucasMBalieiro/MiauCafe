using Item.General;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PedidoDropSlot : MonoBehaviour, IDropHandler
{
    private CatController _catController;
    [SerializeField] private Image backgroundImage;

    public void Start()
    {
        SetBackgroundActive(false);
    }

    public void Initialize(CatController catController)
    {
        _catController = catController;
    }

    public void SetBackgroundActive(bool isActive)
    {
        backgroundImage.enabled = isActive;
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem droppedItem = eventData.pointerDrag.GetComponent<DraggableItem>();


        if (_catController.DeliverItem(droppedItem.ItemData))
        {
            Destroy(droppedItem.gameObject);
        }
        else
        {
            droppedItem.ReturnToPreviousPosition();
        }
    }
}
