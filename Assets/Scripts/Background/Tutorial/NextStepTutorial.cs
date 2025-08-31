using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NextStepTutorial : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent clickedMachine = new UnityEvent();


    public void OnPointerClick(PointerEventData eventData)
    {
        clickedMachine.Invoke();
    }
}
