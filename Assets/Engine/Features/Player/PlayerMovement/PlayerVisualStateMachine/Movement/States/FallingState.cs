using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Movement.States
{
    using Data;
    using StateMachine;

    internal sealed class FallingState : IBaseVisualMovementState, IState.IWithEnterAction, IState.IWithExitAction
    {
        public Animator Animator { get; }
        public PlayerAnimationData PlayerAnimationData { get; }
        private IBaseVisualMovementState _baseVisualMovementStateImplementation;

        internal FallingState(Animator animator,
            PlayerAnimationData playerAnimationData)
        {
            Animator = animator;
            PlayerAnimationData = playerAnimationData;
        }

        public UniTask OnEnterAsync(CancellationToken cancellation = default)
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnExitAsync(CancellationToken cancellation = default)
        {
            Animator.SetBool(PlayerAnimationData.AirborneParameterHash, false);
            return UniTask.CompletedTask;
        }
    }
}
