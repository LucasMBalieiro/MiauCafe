using System.Collections;
using Item.General;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PedidoDropSlot : MonoBehaviour, IDropHandler
{
    private CatController _catController;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI timerPedidoText;
    
    private Coroutine _timerCoroutine;
    private float timer;

    public void Start()
    {
        SetBackgroundActive(false);
    }

    public void Initialize(CatController catController)
    {
        _catController = catController;
    }

    public void SetBackgroundActive(bool isActive)
    {
        backgroundImage.enabled = isActive;
        timerPedidoText.enabled = isActive;
    }

    public void SetTimer(float timerPedido)
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        
        timer = timerPedido;
        timerPedidoText.text = Mathf.RoundToInt(timerPedido).ToString();
    }

    public void StartTimer()
    {
        _timerCoroutine = StartCoroutine(TimerCoroutine());
    }
    
    private IEnumerator TimerCoroutine()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            timerPedidoText.text = Mathf.CeilToInt(timer).ToString();
            
            yield return null;
        }

        if (timer <= 0)
        {
            _catController.FailRequestAndLeave();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem droppedItem = eventData.pointerDrag.GetComponent<DraggableItem>();


        if (_catController.DeliverItem(droppedItem.ItemData))
        {
            Destroy(droppedItem.gameObject);
        }
        else
        {
            droppedItem.ReturnToPreviousPosition();
        }
    }
}
