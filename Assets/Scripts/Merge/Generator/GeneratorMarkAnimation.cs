using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Merge.Generator
{
    public class GeneratorMarkAnimation : MonoBehaviour
    {
        [SerializeField] private float _animCooldown;
        [SerializeField] private float _duration1;
        [SerializeField] private float _force1;
        [SerializeField] private float _duration2;
        [SerializeField] private float _force2;
        [SerializeField] private Ease _ease1;
        [SerializeField] private Ease _ease2;
        [SerializeField] private int _loopsCount = 3;

        private WaitForSeconds _waitCooldown;

        private void Start()
        {
            _waitCooldown = new WaitForSeconds(_animCooldown);
        }

        private void OnEnable()
        {
            _waitCooldown = new WaitForSeconds(_animCooldown);

            StartCoroutine(PlayAnimation());
        }

        private void OnDisable()
        {
            StopCoroutine(PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            while (true)
            {
                yield return _waitCooldown;
                var seq = DOTween.Sequence();

                seq.Append(transform
                        .DOScale(Vector3.one * _force1, _duration1)
                        .SetEase(_ease1))
                    .Append(transform
                        .DOScale(Vector3.one * _force2, _duration2)
                        .SetEase(_ease2))
                    .SetLoops(_loopsCount);
            }
        }
    }
}