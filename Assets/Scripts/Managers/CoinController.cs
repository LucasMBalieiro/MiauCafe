using TMPro;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    
    [SerializeField] private int coins;
    [SerializeField] private TMP_Text coinText;
    
    public static CoinController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        coinText.text = coins.ToString();
    }
    
    public int GetCoins()
    {
        return coins;
    }

    public bool CanBuyItem(int price)
    {
        return coins >= price;
    }


    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCounter();
    }

    public void RemoveCoins(int amount)
    {
        coins -= amount;
        UpdateCounter();
    }
}
