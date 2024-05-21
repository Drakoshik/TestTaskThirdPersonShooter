#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TPS.Engine.Features.Player.PlayerWeapons
{
    using DamageSystem;
    using PlayerMovement;
    internal sealed class Weapon : MonoBehaviour
    {
        [SerializeField] private PlayerInputHolder _inputHolder = default!;
        [field: SerializeField] public WeaponAmmo Ammo { get; private set; } = default!;
        [SerializeField] private WeaponBloom _bloom = default!;
        [SerializeField] private float _fireRate = .15f;

        [SerializeField] private Transform _barrelPosition = default!;
        [SerializeField] private Transform _aim = default!;
        [SerializeField] private AudioClip _gunShot = default!;
        [SerializeField] private AudioSource _audioSource = default!;

        [field: SerializeField] public Transform LeftHandHint { get; private set; } = default!;
        [field: SerializeField] public Transform LeftHandTarget { get; private set; } = default!;

        private Light _muzzleFlashLight = default!;
        private ParticleSystem _muzzleFlashParticles = default!;
        private float _lightIntensity;

        [SerializeField] private float _shootingRange = 100f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private LayerMask _hitLayers;

        private WeaponManager _weaponManager = default!;

        private CancellationTokenSource? _performShoot;

        private void OnValidate()
        {
            _inputHolder = GetComponentInParent<PlayerInputHolder>();
            _weaponManager = GetComponentInParent<WeaponManager>();
            Ammo = GetComponent<WeaponAmmo>();
            _muzzleFlashLight = GetComponentInChildren<Light>();
            _muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            _muzzleFlashLight.intensity = 0;
            _lightIntensity = _muzzleFlashLight.intensity;

            _inputHolder.PlayerActions.Shoot.started += StartShooting;
            _inputHolder.PlayerActions.Reload.started += StopShooting;
            _inputHolder.PlayerActions.Scroll.started += StopShooting;
            _inputHolder.PlayerActions.WeaponSlot1.started += StopShooting;
            _inputHolder.PlayerActions.WeaponSlot2.started += StopShooting;
            _inputHolder.PlayerActions.WeaponSlot3.started += StopShooting;
            _inputHolder.PlayerActions.Shoot.canceled += StopShooting;
        }

        private void OnDestroy()
        {
            _inputHolder.PlayerActions.Shoot.started -= StartShooting;
            _inputHolder.PlayerActions.Shoot.canceled -= StopShooting;
            _inputHolder.PlayerActions.Scroll.started -= StopShooting;
            _inputHolder.PlayerActions.WeaponSlot1.started -= StopShooting;
            _inputHolder.PlayerActions.WeaponSlot2.started -= StopShooting;
            _inputHolder.PlayerActions.WeaponSlot3.started -= StopShooting;
            _performShoot?.Cancel();
            _performShoot?.Dispose();
            _performShoot = null;

        }

        private void OnDisable()
        {
            _performShoot?.Cancel();
            _performShoot?.Dispose();
            _performShoot = null;
        }

        private void StartShooting(InputAction.CallbackContext obj)
        {
            if(gameObject.activeSelf is false) return;
            if(_weaponManager.CanDoAnything is false) return;
            if (_performShoot != null) return;

            _performShoot = new CancellationTokenSource();
            ShootAutomaticAsync(TimeSpan.FromSeconds(_fireRate), _performShoot.Token).Forget();
        }

        private void StopShooting(InputAction.CallbackContext obj)
        {
            _performShoot?.Cancel();
            _performShoot?.Dispose();
            _performShoot = null;
        }

        private async UniTaskVoid ShootAutomaticAsync(TimeSpan rate, CancellationToken cancellation = default)
        {
            while (cancellation.IsCancellationRequested is not true)
            {
                if(Ammo.GetCurrentAmmo() > 0)
                    Shoot();
                await UniTask.Delay(TimeSpan.FromSeconds(.01f), cancellationToken: cancellation);
                _muzzleFlashLight.intensity = 0;
                _lightIntensity = _muzzleFlashLight.intensity;

                if (await UniTask.Delay(rate, DelayType.Realtime, PlayerLoopTiming.FixedUpdate, cancellation, cancelImmediately: true)
                        .SuppressCancellationThrow()) return;
            }
        }

        private void Shoot()
        {
            var screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
            var ray = Camera.main.ScreenPointToRay(screenCentre);

            Debug.DrawRay(ray.origin, ray.direction * _shootingRange, Color.red, 1f);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                var target = hit.transform.GetComponent<DamageTaker>();
                if (target != null)
                {
                    target.TakeDamage(_damage);
                }
            }

            Ammo.SpendAmmo(1);
            _barrelPosition.LookAt(_aim);
            _barrelPosition.localEulerAngles = _bloom.BloomAngle(_barrelPosition);
            _audioSource.PlayOneShot(_gunShot);
            TriggerMuzzleFlash();
        }

        void TriggerMuzzleFlash()
        {
            _muzzleFlashParticles.Play();
            _muzzleFlashLight.intensity = _lightIntensity;
        }

    }
}
