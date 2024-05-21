using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Movement.States
{
    using Data;
    using StateMachine;

    internal interface IBaseVisualMovementState : IState
    {
        Animator Animator { get; }
        PlayerAnimationData PlayerAnimationData { get; }
    }
}
