using UnityEngine;
using Item;
using Managers;
using UnityEngine.UI;

public class Pedido : MonoBehaviour
{
    public Image pedidoImagem;

    public void SetPedido(ItemID pedidoID)
    {
        pedidoImagem.sprite = SpriteManager.Instance.GetSpriteForItem(pedidoID);
    }
}
