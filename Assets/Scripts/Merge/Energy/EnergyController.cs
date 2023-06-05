using System;
using System.Collections;
using UnityEngine;

namespace Merge.Energy
{
    public class EnergyController : MonoBehaviour
    {
        [SerializeField] private int _maxStartEnergy = 100;
        [SerializeField] private float _timeBtwRestoreOneEnergyPoint = 2f;
        [SerializeField] private int _maxSuperEnergyCount = 999;

        private bool _restoreCoroutineWasStarted;
        private bool _toNormalEnergyValueCoroutineWasStarted;
        private int _currentEnergy;

        public int MaxStartEnergy => _maxStartEnergy;
        public float TimeToRestoreOneEnergyInSeconds => _timeBtwRestoreOneEnergyPoint;

        public int CurrentEnergy
        {
            get => _currentEnergy;
            private set
            {
                _currentEnergy = value > _maxSuperEnergyCount ? _maxSuperEnergyCount : value;

                if (_currentEnergy > _maxStartEnergy)
                {
                    StartCoroutine(BackToNormalEnergy());
                }

                LastEnergyChangingTime = DateTime.UtcNow;
                OnEnergyChangedEvent?.Invoke(CurrentEnergy);
            }
        }

        public DateTime LastEnergyChangingTime { get; set; } = DateTime.UtcNow;

        public static EnergyController Instance { get; private set; }
        public event Action<float> OnEnergyChangedEvent;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(RestoreEnergy());
            StartCoroutine(BackToNormalEnergy());
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
            if (_restoreCoroutineWasStarted)
            {
                yield break;
            }

            _restoreCoroutineWasStarted = true;

            while (CurrentEnergy < _maxStartEnergy)
            {
                CurrentEnergy++;
                yield return new WaitForSeconds(TimeToRestoreOneEnergyInSeconds);
            }

            _restoreCoroutineWasStarted = false;
        }

        private IEnumerator BackToNormalEnergy()
        {
            if (_toNormalEnergyValueCoroutineWasStarted)
            {
                yield break;
            }


            _toNormalEnergyValueCoroutineWasStarted = true;

            while (CurrentEnergy > _maxStartEnergy)
            {
                CurrentEnergy--;
                yield return new WaitForSeconds(TimeToRestoreOneEnergyInSeconds / 2f);
            }

            _toNormalEnergyValueCoroutineWasStarted = false;
        }
    }
}