using System;
using System.Collections;
using TPS.Engine.Features.Player.PlayerWeapons;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

namespace TPS.Engine.Features.Player.PlayerMovement
{
    internal sealed class PlayerAnimationEventTrigger : MonoBehaviour
    {

        [SerializeField] private WeaponManager _currentWeapon;


        [SerializeField] private MultiAimConstraint _rightHandAim;
        [SerializeField] private TwoBoneIKConstraint _leftHandIK;

        public void StartWeaponEvent()
        {
            StartCoroutine(ChangeWeight(0));
        }

        public void EndWeaponEvent()
        {
            StartCoroutine(ChangeWeight(1));
        }

        private IEnumerator ChangeWeight(int weight)
        {
            _currentWeapon.SetCanDoAnything(weight == 1);
            yield return new WaitForEndOfFrame();

            _rightHandAim.weight = weight;
            _leftHandIK.weight = weight;
        }

        public void WeaponReloaded()
        {
            _currentWeapon.CurrentAmmo.Reload();
        }

        public void MagOut()
        {
            _currentWeapon.CurrentAmmo.MagOut();
        }

        public void MagIn()
        {
            _currentWeapon.CurrentAmmo.MagIn();
        }

        public void ReleaseSlide()
        {
            _currentWeapon.CurrentAmmo.ReleaseSlide();
        }

        public void WeaponChange()
        {
            _currentWeapon.ClearChangeWeapon();
        }
    }
}
