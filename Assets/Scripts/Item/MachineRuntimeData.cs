using Managers;
using Scriptables.Item;
using UnityEngine;

namespace Item
{
    public class MachineRuntimeData : MonoBehaviour
    {
        private MachineScriptableObject machine;
        public float _lastChargeRechargeTime;
        
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
            _lastChargeRechargeTime = Time.time;
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
                float timeSinceLastRecharge = Time.time - _lastChargeRechargeTime;
                int chargesToAdd = 0;

                while (timeSinceLastRecharge >= machine.cooldown && CurrentCharges < machine.maxCharges)
                {
                    chargesToAdd++;
                    timeSinceLastRecharge -= machine.cooldown;
                    _lastChargeRechargeTime += machine.cooldown;
                }

                if (chargesToAdd > 0)
                {
                    CurrentCharges += chargesToAdd;
                    CurrentCharges = Mathf.Min(CurrentCharges, machine.maxCharges);
                    OnChargesChanged?.Invoke(CurrentCharges, machine.maxCharges);
                }
            }
            else
            {
                _lastChargeRechargeTime = Time.time; 
            }
        }

        private void UpdateCooldownProgress()
        {
            if (CurrentCharges < machine.maxCharges)
            {
                float progress = (Time.time - _lastChargeRechargeTime) / machine.cooldown;
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
                _lastChargeRechargeTime = Time.time;
                OnChargesChanged?.Invoke(CurrentCharges, machine.maxCharges);
                

                BaseItemScriptableObject producedItem = ItemRegistry.Instance.GetIngredient(
                    machine.producesIngredientType, 
                    DropRates.Instance.CalculateTierDrop(machine.tier)
                );
                return producedItem;
            }
            return null; 
        }
    }
}