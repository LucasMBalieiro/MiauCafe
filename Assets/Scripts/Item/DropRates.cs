using System.Linq;
using UnityEngine;

namespace Item
{
    public class DropRates : MonoBehaviour
    {
        public static DropRates Instance { get; private set; }

        [System.Serializable]
        public class DropRate
        {
            public float[] rate;
        }
        
        [SerializeField] private DropRate[] dropRates;

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

        public int CalculateTierDrop(int tierSpawner)
        {
            return CalculateRandom(dropRates[tierSpawner].rate);
        }

        private int CalculateRandom(float[] rates)
        {
            float random = UnityEngine.Random.Range(0, rates.Sum());

            float chance = 0f;
            for (var i = 0; i < rates.Length; i++)
            {
                chance += rates[i];

                if (!(chance >= random)) continue;
                
                Debug.Log("Drop rate: " + rates[i] + " de " + rates.Sum());
                return i;
            }
            Debug.Log("deu ruim");

            return 0;
        }
    
    
    }
}