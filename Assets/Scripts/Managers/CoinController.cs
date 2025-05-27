using Item;
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

    public bool BuyItem(ItemID itemID)
    {
        var price = itemID.tierPrices[itemID.tier];
        if (coins < price) return false;
        
        Debug.Log($"Preço: {price}");
        coins -= price;
        UpdateCounter();
        return true;
    }

    //talvez tenha que trocar pra algum tipo de clientID
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCounter();
    }
}
