using UnityEngine;

namespace Item.Machine
{
    [CreateAssetMenu(fileName = "DropRatesData", menuName = "Game Data/Drop Rates Data")]
    public class DropRatesData : ScriptableObject
    {
        public DropRates.DropRate[] dropRates; 
    }
}
