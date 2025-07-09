using System;
using UnityEngine;

[Serializable]
public class Day
{
    public CompleteClient[] clientList;
}

[Serializable]
public class CompleteClient
{
    public BaseCatScriptableObject baseCat;
    public PedidoScriptableObject pedidos;
}

[CreateAssetMenu(fileName = "DaySpawnerScriptableObject", menuName = "Pedido/DaySpawnerScriptableObject")]
public class DaySpawnerScriptableObject : ScriptableObject
{
    public Day[] days;
}
