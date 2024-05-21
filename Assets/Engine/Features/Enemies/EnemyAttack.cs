#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TPS.Engine.Features.Enemies
{
    internal sealed class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyGameObject = default!;
        [SerializeField] private bool _playerDetected;
        [SerializeField] private float _damage;

        [SerializeField] private EnemyBullet _bulletPrefab = default!;
        [SerializeField] private Transform _bulletSpawnPoint = default!;

        private GameObject? _player;
        private CancellationTokenSource? _performShoot;

        private void OnDisable()
        {
            _playerDetected = false;
            _performShoot?.Cancel();
            _performShoot?.Dispose();
            _performShoot = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _playerDetected = true;
                _player = other.gameObject;
                _performShoot = new CancellationTokenSource();

                ShootAutomaticAsync(TimeSpan.FromSeconds(1.5f), _performShoot.Token).Forget();
            }
        }
        private async UniTaskVoid ShootAutomaticAsync(TimeSpan rate, CancellationToken cancellation = default)
        {
            while (cancellation.IsCancellationRequested is not true)
            {

                if (_player is not null)
                {
                    var bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.transform.position, Quaternion.identity);
                    bullet.transform.LookAt(_player.transform);
                    bullet.transform.position = _bulletSpawnPoint.transform.position;
                    bullet.Rigidbody.AddForce(((_player.transform.position + Vector3.up) - _bulletSpawnPoint.transform.position).normalized * 10f, ForceMode.Impulse);
                }
                if (await UniTask.Delay(rate, DelayType.Realtime, PlayerLoopTiming.FixedUpdate, cancellation, cancelImmediately: true)
                        .SuppressCancellationThrow()) return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _playerDetected = false;
                _performShoot?.Cancel();
                _performShoot?.Dispose();
                _performShoot = null;
            }
        }

        private void Update()
        {
            if (_playerDetected)
            {
                _enemyGameObject.transform.LookAt(_player.transform);
            }
            else
            {
                _enemyGameObject.transform.rotation = Quaternion.identity;
            }
        }
    }

}
