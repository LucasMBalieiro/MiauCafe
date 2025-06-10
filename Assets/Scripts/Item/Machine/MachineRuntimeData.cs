using Item.General;
using Managers;
using Scriptables.Item;
using UnityEngine;

namespace Item.Machine
{
    public class MachineRuntimeData : MonoBehaviour
    {
        private MachineScriptableObject machine;
        public float cooldownTimer; 
        
        public event System.Action<int, int> OnChargesChanged; // current, max
        public event System.Action<float> OnCooldownProgress; // 0.0 to 1.0

        public int CurrentCharges { get; private set; }
        public int MaxCharges => machine.maxCharges;
        public float CooldownDuration => machine.cooldown;

        public void Initialize(MachineScriptableObject machineSO)
        {
            if (machineSO == null)
            {
                Debug.LogError("MachineRuntimeData: MachineSO == null");
                return;
            }
            machine = machineSO;
            CurrentCharges = machine.maxCharges;
            cooldownTimer = Time.time; 
            OnChargesChanged?.Invoke(CurrentCharges, machine.maxCharges);
        }

        private void Update()
        {
            RechargeCharges();
            UpdateCooldownProgress();
        }

        private void RechargeCharges()
        {
            if (CurrentCharges < machine.maxCharges)
            {
                if (Time.time - cooldownTimer >= machine.cooldown)
                {
                    CurrentCharges++;
                    cooldownTimer += machine.cooldown;
                    OnChargesChanged?.Invoke(CurrentCharges, machine.maxCharges);
                }
            }
            else
            {
                cooldownTimer = Time.time; 
            }
        }

        private void UpdateCooldownProgress()
        {
            if (CurrentCharges < machine.maxCharges)
            {
                float progress = (Time.time - cooldownTimer) / machine.cooldown;
                OnCooldownProgress?.Invoke(Mathf.Clamp01(progress));
            }
            else
            {
                OnCooldownProgress?.Invoke(1f);
            }
        }
        
        public BaseItemScriptableObject TryProduceItem()
        {
            if (CurrentCharges > 0)
            {
                CurrentCharges--;
                
                if (CurrentCharges == machine.maxCharges - 1)
                {
                    cooldownTimer = Time.time;
                }
                
                OnChargesChanged?.Invoke(CurrentCharges, machine.maxCharges);

                BaseItemScriptableObject producedItem = ItemRegistry.GetIngredient(
                    machine.producesIngredientType, 
                    DropRates.CalculateTierDrop(machine.tier)
                );
                return producedItem;
            }
            return null; 
        }
    }
}