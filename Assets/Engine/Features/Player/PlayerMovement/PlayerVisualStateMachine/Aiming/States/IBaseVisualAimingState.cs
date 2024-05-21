using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Aiming.States
{
    using Data;
    using StateMachine;

    internal interface IBaseVisualAimingState : IState
    {
        Animator Animator { get; }
        PlayerAnimationData PlayerAnimationData { get; }
    }
}
