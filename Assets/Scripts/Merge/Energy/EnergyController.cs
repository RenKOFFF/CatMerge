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


        public void AddEnergy(int count)
        {
            CurrentEnergy += count;
            if (CurrentEnergy > _maxStartEnergy)
                CurrentEnergy = _maxStartEnergy;

            Debug.Log($"Energy was added on {count} points, current energy: {CurrentEnergy}");
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

            if (CurrentEnergy > _maxStartEnergy) 
                CurrentEnergy = _maxStartEnergy;
            
            _coroutineWasStarted = false;
        }
    }
}