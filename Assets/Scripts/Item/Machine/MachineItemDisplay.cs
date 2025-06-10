using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Item.Machine
{
    public class MachineItemDisplay : MonoBehaviour
    {
        [Header("Machine Specific Visuals")]
        [SerializeField] private TMP_Text _chargesText;
        [SerializeField] private Slider _cooldownSlider;
        
        private MachineRuntimeData _machineRuntimeData;

        private void OnDestroy()
        {
            if (_machineRuntimeData == null) return;
            
            _machineRuntimeData.OnChargesChanged -= UpdateChargeVisuals;
            _machineRuntimeData.OnCooldownProgress -= UpdateCooldownVisuals;
        }
        
        public void Initialize(MachineRuntimeData machineData)
        {
            _machineRuntimeData = machineData;
            
            SetVisualsActive(true);
            
            _machineRuntimeData.OnChargesChanged -= UpdateChargeVisuals;
            _machineRuntimeData.OnCooldownProgress -= UpdateCooldownVisuals;
            _machineRuntimeData.OnChargesChanged += UpdateChargeVisuals;
            _machineRuntimeData.OnCooldownProgress += UpdateCooldownVisuals;

            UpdateChargeVisuals(_machineRuntimeData.CurrentCharges, _machineRuntimeData.MaxCharges);

            float initialProgress = (_machineRuntimeData.CurrentCharges >= _machineRuntimeData.MaxCharges) ? 1f : (Time.time - _machineRuntimeData.cooldownTimer) / _machineRuntimeData.CooldownDuration;
            UpdateCooldownVisuals(initialProgress);
        }
        
        public void SetVisualsActive(bool active)
        {
            if (_chargesText != null) _chargesText.gameObject.SetActive(active);
            if (_cooldownSlider != null) _cooldownSlider.gameObject.SetActive(active);
        }
        
        private void UpdateChargeVisuals(int current, int max)
        {
            if (_chargesText)
            {
                _chargesText.text = current.ToString();
                //_chargesText.color = (current == 0) ? Color.red : Color.white;
            }
        }
        
        private void UpdateCooldownVisuals(float progress)
        {
            if (_cooldownSlider)
            {
                _cooldownSlider.value = progress;
                _cooldownSlider.gameObject.SetActive(progress < 1f);
            }
        }
    }
}