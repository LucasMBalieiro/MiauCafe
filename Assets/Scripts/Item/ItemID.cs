using UnityEngine;

namespace Item
{
    [System.Serializable] public enum ItemType
    {
        [InspectorName("Cafe")] Cafe = 0,
        [InspectorName("Sorvete")] Sorvete = 1,
        [InspectorName("Bolo")] Bolo = 2,
        [InspectorName("Pao")] Pao = 3,
        
        [InspectorName("Maq.Cafe")] MaqCafe = 10,
        [InspectorName("Maq.Sorvete")] MaqSorvete = 11,
        [InspectorName("Maq.Bolo")] MaqBolo = 12,
        [InspectorName("Maq.Pao")] MaqPao = 13,

    }

    [System.Serializable] public class ItemID
    {
        public ItemType type;
        public int tier;

        public ItemID(ItemType type, int tier) 
        {
            this.type = type;
            this.tier = tier;
        }

        public bool IsEqual(ItemID other)
        {
            return type == other.type && tier == other.tier;
        }

        public bool IsEqualType(ItemID other)
        {
            return type == other.type;
        }

        public bool IsEqualTier(ItemID other)
        {
            return tier == other.tier;
        }

        public bool IsMachine()
        {
            return type is ItemType.MaqCafe or ItemType.MaqSorvete or ItemType.MaqBolo or ItemType.MaqPao;
        }
    }
}