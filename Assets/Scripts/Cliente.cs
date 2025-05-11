using UnityEngine;
using Item;
using UnityEngine.UI;
using SpriteHandler;

public class Cliente : MonoBehaviour
{
    public GameObject prefabPedido;
    public Transform listaPedido;
    private GameObject pedidoUI;
    private ItemID pedidoID;
    private bool fezPedido;
    void Start()
    {
        pedidoID = new ItemID(0, 1);
        fezPedido = false;
    }

    public void OnMouseUpAsButton()
    {
        if(!fezPedido) 
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
            pedidoUI = Instantiate(prefabPedido, listaPedido);
            // pedidoUI.GetComponentInChildren<Image>().sprite = SpriteHandler.GetSpriteForItem(pedidoID);
        }
        
    }
}
