using System;
using UnityEngine;

namespace TPS.Engine.Features.Player.Data
{
    [Serializable]
    public class PlayerAnimationData
    {
        [Header("State Group Parameter Names")]
        [SerializeField] private string _groundedParameterName = "Grounded";
        [SerializeField] private string _movingParameterName = "Moving";
        [SerializeField] private string _airborneParameterName = "Airborne";

        [Header("Grounded Parameter Names")]
        [SerializeField] private string _idleParameterName = "IsIdling";
        [SerializeField] private string _runParameterName = "IsRunning";

        [Header("Grounded Parameter Names")]
        [SerializeField] private string _moveXParameterName = "MoveX";
        [SerializeField] private string _moveYParameterName = "MoveY";

        [Header("Weapon Parameter Names")]
        [SerializeField] private string _aimingParameterName = "Aiming";
        [SerializeField] private string _reloadParameterName = "Reload";
        [SerializeField] private string _swapWeaponParameterName = "SwapWeapon";

        public int GroundedParameterHash { get; private set; }
        public int MovingParameterHash { get; private set; }
        public int AirborneParameterHash { get; private set; }
        public int IdleParameterHash { get; private set; }
        public int RunParameterHash { get; private set; }

        public int MoveXParameterHash { get; private set; }
        public int MoveYParameterHash { get; private set; }

        public int AimingParameterHash { get; private set; }
        public int ReloadParameterHash { get; private set; }
        public int SwapWeaponParameterHash { get; private set; }

        public void Initialize()
        {
            GroundedParameterHash = Animator.StringToHash(_groundedParameterName);
            MovingParameterHash = Animator.StringToHash(_movingParameterName);

            AirborneParameterHash = Animator.StringToHash(_airborneParameterName);

            IdleParameterHash = Animator.StringToHash(_idleParameterName);
            RunParameterHash = Animator.StringToHash(_runParameterName);

            MoveXParameterHash = Animator.StringToHash(_moveXParameterName);
            MoveYParameterHash = Animator.StringToHash(_moveYParameterName);

            AimingParameterHash = Animator.StringToHash(_aimingParameterName);
            ReloadParameterHash = Animator.StringToHash(_reloadParameterName);
            SwapWeaponParameterHash = Animator.StringToHash(_swapWeaponParameterName);
        }

    }
}
