using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

namespace TPS.Engine.Features.Player.PlayerWeapons
{
    internal sealed class WeaponManager : MonoBehaviour
    {
        [field: SerializeField] public Weapon CurrentWeapon { get; private set; }
        [field: SerializeField] public WeaponAmmo CurrentAmmo { get; private set; }

        [field: SerializeField, ReadOnly] public bool CanDoAnything { get; private set; }

        [SerializeField] private TwoBoneIKConstraint _leftHandIK;

        [SerializeField] private Weapon[] _weapons;
        [ShowInInspector, ReadOnly] private int _currentWeaponIndex;
        [ShowInInspector, ReadOnly] private int _newWeaponIndex;

        private void Awake()
        {
            _currentWeaponIndex = 0;
            _newWeaponIndex = _currentWeaponIndex;
            for(int i=0; i< _weapons.Length; i++)
            {
                if (i == 0) _weapons[i].gameObject.SetActive(true);
                else _weapons[i].gameObject.SetActive(false);
            }
            CurrentWeapon = _weapons[_currentWeaponIndex];
            CurrentAmmo = _weapons[_currentWeaponIndex].Ammo;
            SetCurrentWeapon(CurrentWeapon);
            CanDoAnything = true;
        }

        public void SetCanDoAnything(bool canDoAnything)
        {
            CanDoAnything = canDoAnything;
        }

        private void SetCurrentWeapon(Weapon weapon)
        {
            _leftHandIK.data.target = weapon.LeftHandHint;
            _leftHandIK.data.hint = weapon.LeftHandHint;
        }

        public void ClearChangeWeapon ()
        {
            if(_newWeaponIndex == _currentWeaponIndex) return;

            _weapons[_currentWeaponIndex].gameObject.SetActive(false);

            _currentWeaponIndex = _newWeaponIndex;

            _weapons[_currentWeaponIndex].gameObject.SetActive(true);
            CurrentWeapon = _weapons[_currentWeaponIndex];
            CurrentAmmo = _weapons[_currentWeaponIndex].Ammo;
            SetCurrentWeapon(CurrentWeapon);
        }

        public void ChangeWeaponIndexFromDirection(float direction)
        {
            int tempIndex;
            if (direction < 0)
            {
                if (_currentWeaponIndex == 0) tempIndex = _weapons.Length - 1;
                else tempIndex = _currentWeaponIndex - 1;
            }
            else
            {
                if (_currentWeaponIndex == _weapons.Length - 1) tempIndex = 0;
                else tempIndex = _currentWeaponIndex + 1;
            }
            _newWeaponIndex = tempIndex;
        }

        public void ChangeWeaponIndex(int mewWeaponIndex)
        {
            _newWeaponIndex = mewWeaponIndex;
        }
    }
}
