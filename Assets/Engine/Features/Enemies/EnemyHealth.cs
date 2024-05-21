using System;
using UnityEngine;
using UnityEngine.Events;

namespace TPS.Engine.Features.Enemies
{
    internal sealed class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _health;
        [SerializeField] private EnemyHealthVisual _healthVisual;

        public UnityEvent OnDie;

        private void OnEnable()
        {
            _health = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _healthVisual.SetBar(_health, _maxHealth);
            if (_health <= 0)
            {
                Die();
                OnDie?.Invoke();
            }
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }
    }
}
