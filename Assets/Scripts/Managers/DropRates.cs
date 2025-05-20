using System.Linq;
using Item;
using UnityEngine;

namespace Managers
{
    public class DropRates : MonoBehaviour
    {
        public static DropRates Instance { get; private set; }

        [System.Serializable]
        public class DropRate
        {
            public int tier;
            public int[] rate;
        }
        
        [SerializeField] private DropRate[] dropRate;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void CalculateChance()
        {
            
        }
        
    
    
    }
}