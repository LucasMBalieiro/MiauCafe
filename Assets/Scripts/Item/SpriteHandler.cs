using System.Linq;
using Item;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteHandler", menuName = "Scriptables/Sprite Handler")]
public class SpriteHandler : ScriptableObject
{
    [System.Serializable]
    public class ItemSprite
    {
        public ItemType type;
        public Sprite[] tierSprites; 
    }
    public ItemSprite[] itemSprites;

    public bool TierExists(ItemType type, int targetTier)
    {
        return (from item in itemSprites where item.type == type select targetTier >= 0 && targetTier < item.tierSprites.Length).FirstOrDefault();
    }
    
    public Sprite GetSpriteForItem(ItemID itemID)
    {
        return (from item in itemSprites where item.type == itemID.type select item.tierSprites[itemID.tier]).FirstOrDefault();
    }
}