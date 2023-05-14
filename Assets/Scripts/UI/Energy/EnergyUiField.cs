using Merge.Energy;
using TMPro;
using UnityEngine;

namespace UI.Energy
{
    public class EnergyUiField : CurrencyFillElement
    {
        private void Start()
        {
            var energyController = EnergyController.Instance;
            energyController.OnEnergyChangedEvent += OnEnergyChanged;
            Initialize(energyController.MaxStartEnergy, energyController.CurrentEnergy);
        }

        private void OnDestroy()
        {
            EnergyController.Instance.OnEnergyChangedEvent -= OnEnergyChanged;
        }

        private void OnEnergyChanged(float currentEnergy)
        {
            ChangeValue(currentEnergy);
        }
    }
}
