using System.Collections.Generic;
using System.Linq;
using Item;
using UnityEngine;

namespace Managers
{
    public class SpriteDictionary : MonoBehaviour
    {
        public static SpriteDictionary Instance { get; private set; }
    
        [System.Serializable]
        public class ItemSprite
        {
            public ItemType type;
            public Sprite[] tierSprites; 
        }

        [Header("Dicionario dos Sprites")]
        [SerializeField] private ItemSprite[] itemSprites;
        private Dictionary<ItemType, Sprite[]> _spriteLookup;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        
            _spriteLookup = itemSprites.ToDictionary(
                item => item.type, 
                item => item.tierSprites
            );
        }
    
    
        public bool TierExists(ItemID itemID)
        {
            if (!_spriteLookup.TryGetValue(itemID.type, out var tiers))
                return false;

            return itemID.tier+1 < tiers.Length;
        }

        public Sprite GetSpriteForItem(ItemID itemID)
        {
            return !_spriteLookup.TryGetValue(itemID.type, out var tiers) ? null : tiers[itemID.tier];
        }
    }
}