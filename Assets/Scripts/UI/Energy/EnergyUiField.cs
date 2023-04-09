using System;
using System.Collections;
using System.Collections.Generic;
using Merge;
using Merge.Energy;
using TMPro;
using UnityEngine;

public class EnergyUiField : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        EnergyController.Instance.OnEnergyChangedEvent += OnEnergyChanged;
    }

    private void OnDestroy()
    {
        EnergyController.Instance.OnEnergyChangedEvent -= OnEnergyChanged;
    }

    private void OnEnergyChanged(float currentEnergy)
    {
        _text.text = currentEnergy.ToString();
    }
}
