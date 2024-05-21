using UnityEngine;

namespace TPS.Engine.Features.Player.PlayerWeapons
{
    internal sealed class WeaponAmmo : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private int _clipSize;
        [SerializeField] private int _currentAmmo;

        [SerializeField] private AudioClip _magInSound;
        [SerializeField] private AudioClip _magOutSound;
        [SerializeField] private AudioClip _releaseSlideSound;

        private void OnValidate()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            _currentAmmo = _clipSize;
        }

        internal void Reload()
        {
            _currentAmmo = _clipSize;
        }

        internal void SpendAmmo(int amount)
        {
            _currentAmmo -= amount;
        }

        internal int GetCurrentAmmo()
        {
            return _currentAmmo;
        }

        public void MagOut()
        {
            _audioSource.PlayOneShot(_magOutSound);
        }

        public void MagIn()
        {
            _audioSource.PlayOneShot(_magInSound);

        }

        public void ReleaseSlide()
        {
            _audioSource.PlayOneShot(_releaseSlideSound);
        }
    }
}
