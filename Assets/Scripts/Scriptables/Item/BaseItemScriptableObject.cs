using UnityEngine;

namespace Scriptables.Item
{
    public enum IngredientType
    {
        Cafe,
        Sorvete,
        Bolo,
        Pao
    }

    public enum MachineType
    {
        MaquinaCafe,
        MaquinaSorvete,
        MaquinaBolo,
        MaquinaPao
    }
    
    
    public enum ItemCategory
    {
        Ingredient,
        Machine
    }
    
    public abstract class BaseItemScriptableObject : ScriptableObject
    {
        public int tier;
        public Sprite sprite;
        
        public abstract ItemCategory Category { get; }
        
    }
}