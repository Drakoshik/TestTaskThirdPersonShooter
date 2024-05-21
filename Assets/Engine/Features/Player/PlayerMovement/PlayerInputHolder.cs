using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerMovement
{
    internal sealed class PlayerInputHolder : MonoBehaviour
    {
        public PlayerInputActions.PlayerActions PlayerActions { get; private set; }

        private PlayerInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            PlayerActions = _inputActions.Player;
            _inputActions.Enable();
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
        }
    }
}
