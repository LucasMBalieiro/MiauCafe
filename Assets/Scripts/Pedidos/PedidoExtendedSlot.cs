using UnityEngine;
using UnityEngine.EventSystems;

public class PedidoExtendedSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private PedidoDropSlot pedidoDropSlot;


    public void OnDrop(PointerEventData eventData)
    {
        pedidoDropSlot.OnDrop(eventData);
    }
}
