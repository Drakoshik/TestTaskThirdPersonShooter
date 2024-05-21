using UnityEngine;
using UnityEngine.Events;

namespace TPS.Engine.Features.DamageSystem
{
    public sealed class DamageTaker : MonoBehaviour
    {
        public UnityEvent<float> OnDamageTaken;

        public void TakeDamage(float damage)
        {
            OnDamageTaken?.Invoke(damage);
        }
    }
}
