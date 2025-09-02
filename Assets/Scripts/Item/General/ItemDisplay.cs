using Scriptables.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Item.General
{
    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField] protected Image _itemImage;
        
        public virtual void SetItem(BaseItemScriptableObject itemData)
        {
            _itemImage.sprite = itemData.sprite;
        }

    }
}