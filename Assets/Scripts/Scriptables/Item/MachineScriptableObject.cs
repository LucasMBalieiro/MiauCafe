using UnityEngine;

namespace Scriptables.Item
{
    [CreateAssetMenu(fileName = "NewMachine", menuName = "Items/Machine")]
    public class MachineScriptableObject : BaseItemScriptableObject
    {
        public MachineType machineType;
        public int cost;
        public float cooldown;
        public int charges;
        
        [Space]
        [Header("Ingrediente")]
        public IngredientType producesIngredientType;

        public override ItemCategory Category => ItemCategory.Machine;
        
    }
}
