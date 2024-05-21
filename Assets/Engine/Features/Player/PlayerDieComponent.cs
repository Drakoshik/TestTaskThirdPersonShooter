using UnityEngine;

namespace TPS.Engine.Features.Player
{
    internal sealed class PlayerDieComponent : MonoBehaviour
    {
        public void Die()
        {
            Debug.Log("Player died");
        }
    }
}
