using System.Collections.Generic;
using System.Linq;
using Managers;
using Scriptables.Item;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public BaseCatScriptableObject CatData { get; private set; }
    [SerializeField] private GameObject pedidoPrefab;
    [SerializeField] private PedidoScriptableObject pedidos;
    
    private CatDisplay _catDisplay;
    private CatAnimation _catAnimation;
    private Transform _gridObject;
    private SpawnPositionController _spawnController;
    private PedidoDropSlot _dropSlot;
    private int _positionIndex;
    
    private Dictionary<BaseItemScriptableObject, int> pedidoDicionario;
    private Dictionary<BaseItemScriptableObject, PedidoDisplay> pedidoDisplays;
    
    [SerializeField] private GameObject pedidoDisplayPrefab;
    
    
    public void Awake()
    {
        _catDisplay = GetComponent<CatDisplay>();
        _catAnimation = GetComponent<CatAnimation>();
        _catAnimation.OnEndTalk.AddListener(ShowPedidoUI);
    }

    public void Initialize(BaseCatScriptableObject newCatData, PedidoScriptableObject newPedidoData, SpawnPositionController spawner, Transform midPosition, Transform endPosition, Transform gridObject, int currentPositionIndex)
    {
        CatData = newCatData;
        pedidos = newPedidoData;
        
        _catDisplay.SetSprite(newCatData);
        _catAnimation.SetPositions(midPosition, endPosition);
        _gridObject = gridObject;
        _positionIndex = currentPositionIndex;
        _spawnController = spawner;
        _dropSlot = _gridObject.GetComponent<PedidoDropSlot>();
        
        SetupPedidos();
        CreatePedidoUI();
    }
    
    private void SetupPedidos()
    {
        pedidoDicionario = new Dictionary<BaseItemScriptableObject, int>();

        foreach (PedidoBase pedido in pedidos.pedidos)
        {
            if (pedidoDicionario.ContainsKey(pedido.ingredient))
            {
                pedidoDicionario[pedido.ingredient] += pedido.quantity;
            }
            else
            {
                pedidoDicionario.Add(pedido.ingredient, pedido.quantity);
            }
        }
    }
    
    private void CreatePedidoUI()
    {
        pedidoDisplays = new Dictionary<BaseItemScriptableObject, PedidoDisplay>();

        foreach (PedidoBase pedido in pedidos.pedidos)
        {
            GameObject displayObject = Instantiate(pedidoDisplayPrefab, _gridObject);
            PedidoDisplay display = displayObject.GetComponent<PedidoDisplay>();
            
            display.Initialize(pedido);
            pedidoDisplays[pedido.ingredient] = display;
        }
        
        
        _dropSlot.SetBackgroundActive(true);
        _gridObject.gameObject.SetActive(false);
    }
    
    private void ShowPedidoUI() => _gridObject.gameObject.SetActive(true);
    
    public bool DeliverItem(BaseItemScriptableObject deliveredItem)
    {
        if (pedidoDicionario.ContainsKey(deliveredItem))
        {
            pedidoDicionario[deliveredItem]--;
            
            Debug.Log($"Delivered {deliveredItem.name}. Remaining needed: {pedidoDicionario[deliveredItem]}");

            int remaining = pedidoDicionario[deliveredItem];
            pedidoDisplays[deliveredItem].UpdateDisplay(remaining);
            
            if (CheckIfRequestIsComplete())
            {
                CompleteRequestAndLeave();
            }
            
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private bool CheckIfRequestIsComplete()
    {
        return pedidoDicionario.Values.All(quantity => quantity <= 0);
    }
    
    public void CompleteRequestAndLeave()
    {

        _spawnController.FreeUpPosition(_positionIndex);
        _dropSlot.SetBackgroundActive(false);
        GameManager.Instance.AddCoins(pedidos.value);
        
        Destroy(gameObject);
    }
}
