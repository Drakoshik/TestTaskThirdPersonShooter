using UnityEngine;

namespace TPS.Engine.Features.Player.Data
{
    [CreateAssetMenu(fileName = "Player",menuName = "Custom/Characters/Player")]
    internal sealed class PlayerSO : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed {get; private set;} = 5f;
        [field: SerializeField] public float SprintSpeed {get; private set;} = 10f;
        [field: SerializeField] public float JumpForce {get; private set;} = 5f;
    }
}
