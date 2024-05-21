using UnityEngine;
using UnityEngine.UI;

namespace TPS.Engine.Features.Enemies
{
    internal sealed class EnemyHealthVisual : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;

        public void SetBar(float health, float maxHealth)
        {
            _healthBar.fillAmount = health / maxHealth;
        }
    }
}
