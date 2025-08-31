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
        [SerializeField] private int currentDay;
        [SerializeField] private DaySpawnerScriptableObject daySpawnerData;
        [SerializeField] private MaxNumberMachines maxNumberMachines;
        
        
        
        private int currentNumCoffeeMachines;
        private int currentNumIceCreamMachines;
        private int currentNumCakeMachines;
        private int currentNumBreadMachines;
        
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
        
        public int GetMaxNumberDay() => daySpawnerData.days.Length;
        
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
        
        /************* MACHINE LIMIT *************/

        public int GetCurrentMachineQuantity(MachineType machineType)
        {
            return machineType switch
            {
                MachineType.MaquinaCafe => currentNumCoffeeMachines,
                MachineType.MaquinaSorvete => currentNumIceCreamMachines,
                MachineType.MaquinaBolo => currentNumCakeMachines,
                MachineType.MaquinaPao => currentNumBreadMachines,
                _ => -1
            };
        }

        public int GetMaxMachineQuantity(MachineType machineType)
        {
            return machineType switch
            {
                MachineType.MaquinaCafe => maxNumberMachines.maxNumberMachinesPerDay[currentDay].maxCoffeeMachine,
                MachineType.MaquinaSorvete => maxNumberMachines.maxNumberMachinesPerDay[currentDay].maxIceCreamMachine,
                MachineType.MaquinaBolo => maxNumberMachines.maxNumberMachinesPerDay[currentDay].maxCakeMachine,
                MachineType.MaquinaPao => maxNumberMachines.maxNumberMachinesPerDay[currentDay].maxBreadMachine,
                _ => -1
            };
        }

        public void AddMachineQuantity(MachineType machineType)
        {
            switch (machineType)
            {
                case MachineType.MaquinaCafe:
                    currentNumCoffeeMachines++;
                    break;
                case MachineType.MaquinaSorvete:
                    currentNumIceCreamMachines++;
                    break;
                case MachineType.MaquinaBolo:
                    currentNumCakeMachines++;
                    break;
                case MachineType.MaquinaPao:
                    currentNumBreadMachines++;
                    break;
            }
        }
        
        /************* DATA PERSISTENCE *************/

        public void LoadData(GameData data)
        {
            this.coins = data.currentCoins;
            this.currentDay = data.currentDay;
            
            this.currentNumCoffeeMachines = data.numCoffeeMachines;
            this.currentNumIceCreamMachines = data.numIceCreamMachines;
            this.currentNumCakeMachines = data.numCakeMachines;
            this.currentNumBreadMachines = data.numBreadMachines;
        }

        public void SaveData(ref GameData data)
        {
            data.currentCoins = coins;
            data.currentDay = currentDay;
            
            data.numCoffeeMachines = currentNumCoffeeMachines;
            data.numIceCreamMachines = currentNumIceCreamMachines;
            data.numCakeMachines = currentNumCakeMachines;
            data.numBreadMachines = currentNumBreadMachines;
        }
    }
}
