using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteHandler", menuName = "Scriptables/Sprite Handler")]
public class SpriteHandler : ScriptableObject
{
    
    public bool TierExists(ItemType type, int targetTier)
    {
        return (from item in itemSprites where item.type == type select targetTier >= 0 && targetTier < item.tierSprites.Length).FirstOrDefault();
    }
    
    [System.Serializable]
    public class ItemSprite
    {
        public ItemType type;
        public Sprite[] tierSprites; 
    }

    public ItemSprite[] itemSprites;

    public Sprite GetSpriteForItem(ItemType type, int tier)
    {
        foreach (var item in itemSprites)
        {
            if (item.type == type)
            {
                if (tier >= 0 && tier < item.tierSprites.Length)
                {
                    return item.tierSprites[tier];
                }
                return item.tierSprites[^1]; // Return last sprite if tier too high
            }
        }
        return null;
    }
}