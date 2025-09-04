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
        _dropSlot.SetTimer(pedidos.timer);
        
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
        
        
        _dropSlot.SetBackgroundActive(false);
        _gridObject.gameObject.SetActive(false);
    }
    
    private void ShowPedidoUI() 
    {
        _dropSlot.SetBackgroundActive(true);
        _gridObject.gameObject.SetActive(true);
        _dropSlot.StartTimer();
    }
    
    public bool DeliverItem(BaseItemScriptableObject deliveredItem)
    {
        if (pedidoDicionario.ContainsKey(deliveredItem))
        {
            pedidoDicionario[deliveredItem]--;
            
            int remaining = pedidoDicionario[deliveredItem];
            pedidoDisplays[deliveredItem].UpdateDisplay(remaining);

            if (remaining == 0)
            {
                pedidoDicionario.Remove(deliveredItem);
            }
            
            //TODO: adicionar som de entrega com sucesso aqui
            
            if (CheckIfRequestIsComplete())
            {
                CompleteRequestAndLeave();
            }
            
            SoundManager.Instance.PlaySFX("Ding");
            
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
        
        SoundManager.Instance.PlaySFX("Deliver");

        _spawnController.FreeUpPosition(_positionIndex);
        _dropSlot.SetBackgroundActive(false);
        _dropSlot.StopTimer();
        GameManager.Instance.AddCoins(pedidos.value);
        
        Destroy(gameObject);
    }

    public void FailRequestAndLeave()
    {
        pedidoDicionario.Clear();

        foreach (Transform pedidoDisplay in _gridObject)
        {
            Destroy(pedidoDisplay.gameObject);
        }
        
        SoundManager.Instance.PlaySFX("Deny");
        _spawnController.FreeUpPosition(_positionIndex);
        _dropSlot.SetBackgroundActive(false);
        
        Destroy(gameObject);
    }
}
