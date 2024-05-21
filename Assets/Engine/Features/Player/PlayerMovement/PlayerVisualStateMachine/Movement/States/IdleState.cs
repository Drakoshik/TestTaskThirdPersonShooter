using System.Threading;
using Cysharp.Threading.Tasks;
using TPS.Engine.Features.Player.Data;
using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Movement.States
{
    using StateMachine;

    internal sealed class IdleState : IBaseVisualMovementState, IState.IWithEnterAction
    {
        public Animator Animator { get; }
        public PlayerAnimationData PlayerAnimationData { get; }


        internal IdleState(Animator animator,
            PlayerAnimationData playerAnimationData)
        {
            Animator = animator;
            PlayerAnimationData = playerAnimationData;
        }

        public UniTask OnEnterAsync(CancellationToken cancellation = default)
        {
            Animator.SetBool(PlayerAnimationData.MovingParameterHash, false);
            return UniTask.CompletedTask;
        }
    }
}
