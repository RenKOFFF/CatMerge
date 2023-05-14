using System;
using System.Collections;
using SaveSystem;
using UnityEngine;

namespace Merge.Energy
{
    public class EnergyController : MonoBehaviour
    {
        [SerializeField] private int _maxStartEnergy = 100;
        [SerializeField] private float _timeBtwRestoreOneEnergyPoint = 30f;

        private bool _coroutineWasStarted;
        private int _currentEnergy;

        public int MaxStartEnergy => _maxStartEnergy;
        public int CurrentEnergy
        {
            get => _currentEnergy;
            private set
            {
                if (value > _maxStartEnergy)
                    _currentEnergy = _maxStartEnergy;
                else _currentEnergy = value;

                OnEnergyChangedEvent?.Invoke(CurrentEnergy);
            }
        }

        public static EnergyController Instance { get; private set; }
        public event Action<float> OnEnergyChangedEvent;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(RestoreEnergy());
        }

        private void Start()
        {
            //CurrentEnergy = _maxStartEnergy;
        }

        public void SetEnergy(int energy)
        {
            CurrentEnergy = energy;
        }

        public bool SpendEnergy()
        {
            if (CurrentEnergy > 0)
            {
                CurrentEnergy--;

                StartCoroutine(RestoreEnergy());

                return true;
            }

            return false;
        }


        public void AddEnergy(int count)
        {
            CurrentEnergy += count;
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
                CurrentEnergy++;
                yield return new WaitForSeconds(_timeBtwRestoreOneEnergyPoint);
            }

            _coroutineWasStarted = false;
        }
    }
}
