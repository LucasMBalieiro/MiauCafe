using Scriptables.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PedidoDisplay : MonoBehaviour
{
    
    
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantity;

    private BaseItemScriptableObject _itemData;
    
    public void Initialize(PedidoBase pedidoData)
    {
        _itemData = pedidoData.ingredient;
        
        icon.sprite = _itemData.sprite;
        quantity.text = "x" + pedidoData.quantity.ToString();

    }

    public void UpdateDisplay(int quantityRemaining)
    {
        quantity.text = "x" + quantityRemaining.ToString();

        if (quantityRemaining == 0)
        {
            Destroy(gameObject);
        }
    }
}
