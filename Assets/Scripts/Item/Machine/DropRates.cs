using System.Linq;
using UnityEngine;

namespace Item.Machine
{
    public static class DropRates
    {
        [System.Serializable]
        public class DropRate
        {
            public float[] rate;
        }
        
        private static DropRate[] _dropRates;
        
        public static void Initialize(DropRate[] initialDropRates)
        {
            _dropRates = initialDropRates;
            Debug.Log("DropRates initialized");
        }
        
        public static int CalculateTierDrop(int tierSpawner)
        {
            return CalculateRandom(_dropRates[tierSpawner].rate);
        }

        private static int CalculateRandom(float[] rates)
        {
            float sumOfRates = rates.Sum();
            float random = UnityEngine.Random.Range(0, sumOfRates);
            float chance = 0f;
            
            for (var i = 0; i < rates.Length; i++)
            {
                chance += rates[i];
                if (chance >= random) return i;
            }
            
            return 0;
        }
    }
}