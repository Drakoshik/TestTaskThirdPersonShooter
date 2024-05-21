using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Movement.States
{
    using Data;
    using StateMachine;
    internal sealed class MovingState : IBaseVisualMovementState, IState.IWithEnterAction, IState.IWithExitAction, IState.IWithUpdateLoop
    {
        public Animator Animator { get; }
        public PlayerAnimationData PlayerAnimationData { get; }
        private readonly PlayerInputHolder _playerInputHolder;
        private IBaseVisualMovementState _baseVisualMovementStateImplementation;

        internal MovingState(PlayerInputHolder playerInputHolder, Animator animator, PlayerAnimationData playerAnimationData)
        {
            LoopTiming = PlayerLoopTiming.Update;
            _playerInputHolder = playerInputHolder;
            Animator = animator;
            PlayerAnimationData = playerAnimationData;
        }

        public UniTask OnEnterAsync(CancellationToken cancellation = default)
        {
            Animator.SetBool(PlayerAnimationData.MovingParameterHash, true);
            return UniTask.CompletedTask;
        }

        public UniTask OnExitAsync(CancellationToken cancellation = default)
        {
            Animator.SetBool(PlayerAnimationData.MovingParameterHash, false);
            return UniTask.CompletedTask;
        }

        public UniTask OnUpdate(CancellationToken cancellation = default)
        {
            var movement = _playerInputHolder.PlayerActions.Movement.ReadValue<Vector2>();
            Animator.SetFloat(PlayerAnimationData.MoveXParameterHash, movement.x);
            Animator.SetFloat(PlayerAnimationData.MoveYParameterHash, movement.y);
            return UniTask.CompletedTask;
        }

        public PlayerLoopTiming LoopTiming { get; }
    }
}
