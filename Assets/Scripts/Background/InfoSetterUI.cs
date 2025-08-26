using Managers;
using TMPro;
using UnityEngine;

public class InfoSetterUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI coinCounter;
    [SerializeField]private TextMeshProUGUI dayCounter;
    
    private void Start()
    {
        UpdateCoinDisplay(GameManager.Instance.GetCoins());
        UpdateDayDisplay(GameManager.Instance.GetCurrentDay());
    }

    public void OnEnable()
    {
        GameManager.OnCoinsChanged += UpdateCoinDisplay;
        GameManager.OnDayChanged += UpdateDayDisplay;
    }

    public void OnDisable()
    {
        GameManager.OnCoinsChanged -= UpdateCoinDisplay;
        GameManager.OnDayChanged -= UpdateDayDisplay;
    }
    
    private void UpdateCoinDisplay(int value)
    {
        coinCounter.text = value.ToString() + " coins";
    }

    private void UpdateDayDisplay(int value)
    {
        dayCounter.text = "Day - " + value.ToString();
    }
}
