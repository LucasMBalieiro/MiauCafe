using System.Collections.Generic;
using Scriptables.Item;
using UnityEngine;

namespace Item.General
{
    [CreateAssetMenu(fileName = "ItemRegistryData", menuName = "Game Data/Item Registry Data")]
    public class ItemRegistryData : ScriptableObject
    {
        public List<BaseItemScriptableObject> items;
    }
}
