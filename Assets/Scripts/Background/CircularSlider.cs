using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircularSlider : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image fillImage;
    [SerializeField] private RectTransform handleTransform;

    [Header("Slider Settings")]
    [SerializeField, Range(0f, 1f)] public float sliderValue = 0.5f;
    [SerializeField] private float radius = 200f; // The radius of the circular path
    [SerializeField] private float startAngle = 180f; // Start angle in degrees
    [SerializeField] private float endAngle = 360f;   // End angle in degrees

    public UnityEvent<float> OnValueChanged;

    private RectTransform rectTransform;
    
    private bool canChange;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateSliderVisuals(sliderValue);
    }

    // Called when the user starts clicking or dragging on the slider
    public void OnDrag(PointerEventData eventData)
    {
        if (canChange)
        {
            UpdateSliderFromInput(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canChange)
        {
            UpdateSliderFromInput(eventData);
        }
    }

    private void UpdateSliderFromInput(PointerEventData eventData)
    {
        // Convert the mouse's screen position to a local position within the RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPos);

        // Calculate the angle of the input position relative to the pivot
        float angle = Mathf.Atan2(localPos.y, localPos.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360; // Convert angle to 0-360 range

        // Clamp the angle to the allowed range (e.g., 180 to 360 for a bottom half-circle)
        float clampedAngle = Mathf.Clamp(angle, startAngle, endAngle);

        // Normalize the angle to a 0-1 value
        sliderValue = (clampedAngle - startAngle) / (endAngle - startAngle);
        
        UpdateSliderVisuals(sliderValue);
        OnValueChanged?.Invoke(sliderValue);
    }

    private void UpdateSliderVisuals(float value)
    {
        // Update the fill amount of the background image
        if (fillImage != null)
        {
            fillImage.fillAmount = value;
        }

        // Update the position of the handle
        if (handleTransform != null)
        {
            float currentAngleRad = Mathf.Lerp(startAngle, endAngle, value) * Mathf.Deg2Rad;
            handleTransform.localPosition = new Vector2(Mathf.Cos(currentAngleRad) * radius, Mathf.Sin(currentAngleRad) * radius);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canChange = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canChange = false;
    }
}