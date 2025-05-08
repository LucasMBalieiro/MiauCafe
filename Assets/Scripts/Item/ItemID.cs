using UnityEngine;

[System.Serializable] public enum ItemType
{
    [InspectorName("Cafe")] Cafe = 0,
    [InspectorName("Sorvete")] Sorvete = 1,
    [InspectorName("Bolo")] Bolo = 2,
    [InspectorName("Pao")] Pao = 3,
    [InspectorName("Numero")] Numero = 9,

}

[System.Serializable] public class ItemID
{
    public ItemType type;
    public int tier;

    // Composite ID that combines type and level
    public string GetCompositeID() => $"{(int)type}_{tier}";

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
    
    
}
