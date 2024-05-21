using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TPS.Engine.Features.Player.PlayerMovement.PlayerVisualStateMachine.Aiming
{
    using StateMachine;
    using Data;
    using States;

    internal sealed class PlayerVisualAimingStateMachine : MonoBehaviour
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
            _playerInputHolder.PlayerActions.Aim.started += StartAimingState;
            _playerInputHolder.PlayerActions.Aim.canceled += StopAimingState;

            _playerInputHolder.PlayerActions.Reload.started += StartReloadingState;

            _playerInputHolder.PlayerActions.Scroll.started += StartSwappingWeaponState;
            _playerInputHolder.PlayerActions.WeaponSlot1.started += StartSwappingWeaponState;
            _playerInputHolder.PlayerActions.WeaponSlot2.started += StartSwappingWeaponState;
            _playerInputHolder.PlayerActions.WeaponSlot3.started += StartSwappingWeaponState;

        }

        private void OnDestroy()
        {
            _playerInputHolder.PlayerActions.Aim.started -= StartAimingState;
            _playerInputHolder.PlayerActions.Aim.canceled -= StopAimingState;

            _playerInputHolder.PlayerActions.Reload.started -= StartReloadingState;

            _playerInputHolder.PlayerActions.Scroll.started -= StartSwappingWeaponState;
            _playerInputHolder.PlayerActions.WeaponSlot1.started -= StartSwappingWeaponState;
            _playerInputHolder.PlayerActions.WeaponSlot2.started -= StartSwappingWeaponState;
            _playerInputHolder.PlayerActions.WeaponSlot3.started -= StartSwappingWeaponState;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void StartAimingState(InputAction.CallbackContext obj)
        {
            HipToAdsAsync(_cancellationTokenSource.Token);
        }

        private void StopAimingState(InputAction.CallbackContext obj)
        {
            AdsToHipAsync(_cancellationTokenSource.Token);
        }

        private void StartReloadingState(InputAction.CallbackContext obj)
        {
            if(_playerInputHolder.PlayerActions.Aim.inProgress)
                AdsToHipAsync(_cancellationTokenSource.Token);

            HipToReloadAsync(_cancellationTokenSource.Token);
        }

        private void StartSwappingWeaponState(InputAction.CallbackContext obj)
        {
            if(_playerInputHolder.PlayerActions.Aim.inProgress)
                AdsToHipAsync(_cancellationTokenSource.Token);

            HipToSwapWeaponAsync(_cancellationTokenSource.Token);
        }

        private void EndReloading()
        {
            ReloadToHipAsync();

            if(_playerInputHolder.PlayerActions.Aim.inProgress)
                HipToAdsAsync(_cancellationTokenSource.Token);
        }

        private void EndSwapWeapon()
        {
            SwapWeaponToHipAsync();

            if(_playerInputHolder.PlayerActions.Aim.inProgress)
                HipToAdsAsync(_cancellationTokenSource.Token);
        }

        private void Init()
        {

            var hipFireState = new HipFireState(_animator, _playerAnimationData);
            var adsState = new AdsState(_animator, _playerAnimationData);
            var swapWeaponState = new SwapWeaponState(_animator, _playerAnimationData);
            var reloadState = new ReloadState(_animator, _playerAnimationData);


            _stateMachine = new StateMachine.Deterministic
            (
                transitions: new KeyValuePair<int, (IState From, IState To)>[]
                {
                    new(StateMachine.CreateKey<Initialize>(), (From: StateMachine.InitialState, To: hipFireState)),
                    new(StateMachine.CreateKey<HipToAds>(), (From: hipFireState, To: adsState)),
                    new(StateMachine.CreateKey<HipToReload>(), (From: hipFireState, To: reloadState)),
                    new(StateMachine.CreateKey<HipToSwapWeapon>(), (From: hipFireState, To: swapWeaponState)),
                    new(StateMachine.CreateKey<AdsToHip>(), (From: adsState, To: hipFireState)),
                    new(StateMachine.CreateKey<ReloadToHip>(), (From: reloadState, To: hipFireState)),
                    new(StateMachine.CreateKey<SwapWeaponToHip>(), (From: swapWeaponState, To: hipFireState)),
                }
            );
        }

        public UniTask InitializeStates(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<Initialize>(cancellation);

        public UniTask HipToAdsAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<HipToAds>(cancellation);

        public UniTask HipToReloadAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<HipToReload>(cancellation);

        public UniTask HipToSwapWeaponAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<HipToSwapWeapon>(cancellation);

        public UniTask AdsToHipAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<AdsToHip>(cancellation);

        public UniTask ReloadToHipAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<ReloadToHip>(cancellation);

        public UniTask SwapWeaponToHipAsync(CancellationToken cancellation = default) =>
            _stateMachine.TransitAsync<SwapWeaponToHip>(cancellation);

        private readonly struct Initialize{}

        private readonly struct HipToAds{}

        private readonly struct HipToReload{}

        private readonly struct HipToSwapWeapon{}

        private readonly struct AdsToHip{}

        private readonly struct ReloadToHip{}

        private readonly struct SwapWeaponToHip{}

    }

}
