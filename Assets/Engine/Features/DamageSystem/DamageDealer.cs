using System;
using UnityEngine;

namespace TPS.Engine.Features.DamageSystem
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private float _damage;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<DamageTaker>(out var damageTaker))
                damageTaker.TakeDamage(_damage);
        }
    }
}
