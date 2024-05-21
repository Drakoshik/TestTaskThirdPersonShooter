using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Aiming.States
{
    using Data;
    using StateMachine;

    internal sealed class AdsState : IBaseVisualAimingState, IState.IWithEnterAction, IState.IWithExitAction
    {
        public Animator Animator { get; }
        public PlayerAnimationData PlayerAnimationData { get; }

        public AdsState(Animator animator, PlayerAnimationData playerAnimationData)
        {
            Animator = animator;
            PlayerAnimationData = playerAnimationData;
        }
        public UniTask OnEnterAsync(CancellationToken cancellation = default)
        {
            Animator.SetBool(PlayerAnimationData.AimingParameterHash, true);
            return UniTask.CompletedTask;
        }

        public UniTask OnExitAsync(CancellationToken cancellation = default)
        {
            Animator.SetBool(PlayerAnimationData.AimingParameterHash, false);
            return UniTask.CompletedTask;
        }
    }
}
