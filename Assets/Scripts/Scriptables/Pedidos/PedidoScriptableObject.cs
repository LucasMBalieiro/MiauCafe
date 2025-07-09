using System;
using Scriptables.Item;
using UnityEngine;

[Serializable] public class PedidoBase
{
    public IngredientScriptableObject ingredient;
    public int quantity = 1;
}

[CreateAssetMenu(fileName = "PedidoScriptableObject", menuName = "Pedido/PedidoBase")]
public class PedidoScriptableObject : ScriptableObject
{
    
    public int value;
    [SerializeField] public PedidoBase[] pedidos;
    
}
