using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Aiming.States
{
    using Data;
    using StateMachine;

    internal sealed class HipFireState : IBaseVisualAimingState, IState.IWithEnterAction
    {
        public Animator Animator { get; }
        public PlayerAnimationData PlayerAnimationData { get; }

        public HipFireState(Animator animator, PlayerAnimationData playerAnimationData)
        {
            Animator = animator;
            PlayerAnimationData = playerAnimationData;
        }
        public UniTask OnEnterAsync(CancellationToken cancellation = default)
        {
            Animator.SetBool(PlayerAnimationData.AimingParameterHash, false);
            return UniTask.CompletedTask;
        }
    }
}
