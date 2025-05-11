using UnityEngine;
using Item;
using UnityEngine.UI;

public class Pedido : MonoBehaviour
{
    public SpriteHandler spriteHandler;
    public Image pedidoImagem;

    public void SetPedido(ItemID pedidoID)
    {
        pedidoImagem.sprite = spriteHandler.GetSpriteForItem(pedidoID);
    }
}
