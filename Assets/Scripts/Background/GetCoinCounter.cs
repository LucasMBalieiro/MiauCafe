using Managers;
using TMPro;
using UnityEngine;

namespace Background
{
    public class GetCoinCounter : MonoBehaviour
    {
        public TextMeshProUGUI coinCounter;

        private void Awake()
        {
            UpdateDisplay(GameManager.Instance.GetCoins());
        }

        public void OnEnable()
        {
            GameManager.OnCoinsChanged += UpdateDisplay;
        }

        public void OnDisable()
        {
            GameManager.OnCoinsChanged -= UpdateDisplay;
        }
    
        private void UpdateDisplay(int value)
        {
            coinCounter.text = value.ToString();
        }
    }
}
