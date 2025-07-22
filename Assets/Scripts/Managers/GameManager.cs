using System;
using DataPersistence;
using Item.General;
using Item.Machine;
using Scriptables.Item;
using UnityEngine;
using UnityEngine.Serialization;


namespace Managers
{
    public class GameManager : MonoBehaviour, IDataPersistence
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private ItemRegistryData itemRegistryData; 
        [SerializeField] private DropRatesData dropRatesData;

        [SerializeField] private int coins;
        [SerializeField]private int currentDay;
        [SerializeField] private DaySpawnerScriptableObject daySpawnerData;
        
        public static event Action<int> OnCoinsChanged;
        public static event Action<int> OnDayChanged;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeGameData();
        }
        
        private void InitializeGameData()
        {
            ItemRegistry.Initialize(itemRegistryData.items);
            DropRates.Initialize(dropRatesData.dropRates);
        }

        
        /************* CONTROLLER DINHEIRO *************/
        
        public int GetCoins() => coins;
        
        public bool CanBuyItem(int price) => coins >= price;
        
        public void AddCoins(int amount)
        {
            coins += amount;
            OnCoinsChanged?.Invoke(coins);
        }

        public void RemoveCoins(int amount)
        {
            coins -= amount;
            OnCoinsChanged?.Invoke(coins);
        }
        
        /************* CONTROLLER SPAWNER DIAS *************/
        
        public int GetCurrentDay() => currentDay;
        
        public Day GetCurrentDayData()
        {
            if (daySpawnerData != null && currentDay < daySpawnerData.days.Length)
            {
                return daySpawnerData.days[currentDay];
            }

            Debug.LogError($"Não possui o dia: {currentDay}");
            return null;
        }

        public void ChangeDay() 
        {
            currentDay++;
            OnDayChanged?.Invoke(currentDay);
            DataPersistenceManager.Instance.SaveGame();
        }

        public void LoadData(GameData data)
        {
            this.coins = data.currentCoins;
            this.currentDay = data.currentDay;
        }

        public void SaveData(ref GameData data)
        {
            data.currentCoins = coins;
            data.currentDay = currentDay;
        }
    }
}
