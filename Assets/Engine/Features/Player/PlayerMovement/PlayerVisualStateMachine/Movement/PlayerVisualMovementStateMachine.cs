using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Movement
{
    using StateMachine;
    using Data;
    using States;

    [Serializable]
    public class PlayerVisualMovementStateMachine : MonoBehaviour
    {
        [SerializeField] private Animator _animator = default!;
        [SerializeField] private PlayerInputHolder _playerInputHolder = default!;

        private StateMachine _stateMachine;
        private CancellationTokenSource _cancellationTokenSource;
        private PlayerAnimationData _playerAnimationData;

        private void Awake()
        {
            _playerAnimationData = new PlayerAnimationData();
            _playerAnimationData.Initialize();
            Init();
            _cancellationTokenSource = new CancellationTokenSource();
            InitializeStates(_cancellationTokenSource.Token);
        }
        private void Start()
        {
            _playerInputHolder.PlayerActions.Movement.started += StartMovingState;
            _playerInputHolder.PlayerActions.Movement.canceled += StopMovingState;

            _playerInputHolder.PlayerActions.Sprint.started += StartRunningState;
            _playerInputHolder.PlayerActions.Sprint.canceled += StopRunningState;

            _playerInputHolder.PlayerActions.Jump.started += StartJumpState;
        }

        private void OnDestroy()
        {
            _playerInputHolder.PlayerActions.Movement.started -= StartMovingState;
            _playerInputHolder.PlayerActions.Movement.canceled -= StopMovingState;

            _playerInputHolder.PlayerActions.Sprint.started -= StartRunningState;
            _playerInputHolder.PlayerActions.Sprint.canceled -= StopRunningState;

            _playerInputHolder.PlayerActions.Jump.started -= StartJumpState;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }


        private void StartMovingState(InputAction.CallbackContext obj)
        {
            StartMovingAsync(_cancellationTokenSource.Token);
        }
        private void StopMovingState(InputAction.CallbackContext obj)
        {
            if (_playerInputHolder.PlayerActions.Sprint.inProgress)
            {
                StopRunningAsync(_cancellationTokenSource.Token);
            }

            StopMovingAsync(_cancellationTokenSource.Token);
        }

        private void StartRunningState(InputAction.CallbackContext obj)
        {
            StartRunningAsync(_cancellationTokenSource.Token);
        }
        private void StopRunningState(InputAction.CallbackContext obj)
        {
            StopRunningAsync(_cancellationTokenSource.Token);
        }

        private void StartJumpState(InputAction.CallbackContext obj)
        {
            switch (_stateMachine.Current)
            {
                case IdleState:
                    StartIdleJumpAsync(_cancellationTokenSource.Token);
                    break;
                case MovingState:
                    StartWalkJumpAsync(_cancellationTokenSource.Token);
                    break;
                case RunningState:
                    StartRunJumpAsync(_cancellationTokenSource.Token);
                    break;
            }
        }

        public void Land()
        {
            switch (_stateMachine.Current)
            {
                case RunJumpState:
                    Debug.Log("LandFromRunJumpAsync");
                    LandFromRunJumpAsync(_cancellationTokenSource.Token);
                    break;
                case IdleJumpState:
                    Debug.Log("LandFromIdleJumpAsync");
                    LandFromIdleJumpAsync(_cancellationTokenSource.Token);
                    break;
            }

            if (!_playerInputHolder.PlayerActions.Movement.inProgress) return;

            StartMovingAsync(_cancellationTokenSource.Token);

            if (!_playerInputHolder.PlayerActions.Sprint.inProgress) return;

            StartRunningAsync(_cancellationTokenSource.Token);
        }

        private void Init()
        {
            var idleState = new IdleState(_animator, _playerAnimationData);

            var movingState = new MovingState(_playerInputHolder, _animator, _playerAnimationData);
            var runningState = new RunningState(_playerInputHolder, _animator, _playerAnimationData);

            var idleJumpState = new IdleJumpState(_animator, _playerAnimationData);
            var runJumpState = new RunJumpState(_animator, _playerAnimationData);

            _stateMachine = new StateMachine.Deterministic
            (
                transitions: new KeyValuePair<int, (IState From, IState To)>[]
                {
                    new(StateMachine.CreateKey<Initialize>(), (From: StateMachine.InitialState, To: idleState)),

                    new(StateMachine.CreateKey<StartMoving>(), (From: idleState, To: movingState)),
                    new(StateMachine.CreateKey<StopMoving>(), (From: movingState, To: idleState)),

                    new(StateMachine.CreateKey<StartRun>(), (From: movingState, To: runningState)),
                    new(StateMachine.CreateKey<StopRun>(), (From: runningState, To: movingState)),

                    new(StateMachine.CreateKey<IdleJump>(), (From: idleState, To: idleJumpState)),
                    new(StateMachine.CreateKey<WalkJump>(), (From: movingState, To: runJumpState)),
                    new(StateMachine.CreateKey<RunJump>(), (From: runningState, To: runJumpState)),

                    new(StateMachine.CreateKey<FallingFromIdle>(), (From: idleJumpState, To: idleState)),
                    new(StateMachine.CreateKey<FallingFromRun>(), (From: runJumpState, To: idleState)),

                }
            );
        }

        public UniTask InitializeStates(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<Initialize>(cancellation);

        public UniTask StartMovingAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<StartMoving>(cancellation);

        public UniTask StopMovingAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<StopMoving>(cancellation);

        public UniTask StartRunningAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<StartRun>(cancellation);

        public UniTask StopRunningAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<StopRun>(cancellation);

        public UniTask StartIdleJumpAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<IdleJump>(cancellation);

        public UniTask StartWalkJumpAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<WalkJump>(cancellation);

        public UniTask StartRunJumpAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<RunJump>(cancellation);

        public UniTask LandFromIdleJumpAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<FallingFromIdle>(cancellation);

        public UniTask LandFromRunJumpAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<FallingFromRun>(cancellation);



        private readonly struct Initialize{}

        private readonly struct StartMoving{}

        private readonly struct StopMoving{}

        private readonly struct StartRun{}

        private readonly struct StopRun{}

        private readonly struct IdleJump{}

        private readonly struct WalkJump{}

        private readonly struct RunJump{}

        private readonly struct FallingFromIdle{}
        private readonly struct FallingFromRun{}

    }
}
