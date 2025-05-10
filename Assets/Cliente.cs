using UnityEngine;
using Item;
using Unity.VisualScripting;

public class Cliente : MonoBehaviour
{
    private ItemID pedidoID;
    private bool fezPedido;
    void Start()
    {
        pedidoID = new ItemID(0, 1);
        fezPedido = false;
    }

    public void OnMouseUpAsButton()
    {
        // if(!fezPedido) CriarPedido(pedidoID);
        gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
    }
}
