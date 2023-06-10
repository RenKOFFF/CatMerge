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
            if (currentEnergy > EnergyController.Instance.MaxStartEnergy)
                ChangeMaxValueVisibility(true);
            else ChangeMaxValueVisibility(false);

            ChangeValue(currentEnergy);
        }
    }
}