using System;
using System.Collections;
using UnityEngine;

namespace Merge
{
    public class EnergyController : MonoBehaviour
    {
        [SerializeField] private float _maxStartEnergy = 10f;
        [SerializeField] private float _timeBtwRestoreOneEnergyPoint = 3f;

        private bool _coroutineWasStarted;

        public float CurrentEnergy { get; private set; }
        public static EnergyController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CurrentEnergy = _maxStartEnergy;
        }

        public bool SpendEnergy()
        {
            if (CurrentEnergy > 0)
            {
                CurrentEnergy--;
                Debug.Log($"Energy was spended, current energy: {CurrentEnergy}");

                StartCoroutine(RestoreEnergy());

                return true;
            }

            Debug.Log("No Energy");

            return false;
        }

        private IEnumerator RestoreEnergy()
        {
            if (_coroutineWasStarted)
            {
                yield break;
            }

            _coroutineWasStarted = true;
            
            while (CurrentEnergy < _maxStartEnergy)
            {
                yield return new WaitForSeconds(_timeBtwRestoreOneEnergyPoint);
                CurrentEnergy++;
                Debug.Log($"CurrentEnergy: {CurrentEnergy}");
            }

            _coroutineWasStarted = false;
        }
    }
}