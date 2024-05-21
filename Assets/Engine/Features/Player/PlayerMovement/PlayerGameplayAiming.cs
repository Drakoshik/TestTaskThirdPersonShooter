using System;
using Cinemachine;
using TPS.Engine.Features.Player.PlayerWeapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TPS.Engine.Features.Player.PlayerMovement
{
    internal sealed class PlayerGameplayAiming : MonoBehaviour
    {
        [SerializeField] private WeaponManager _weaponManager = default!;

        [SerializeField] private Transform _aimTarget = default!;
        [SerializeField] private PlayerInputHolder _inputHolder = default!;
        [SerializeField] private CinemachineVirtualCamera _camera = default!;

        [SerializeField] private Transform _aimPosition = default!;
        [SerializeField] private float _aimSmoothSpeed = 20;
        [SerializeField] private LayerMask _aimMask;

        [SerializeField] private float _adsFov = 40;
        [SerializeField] private float _hipFov = 70;
        private float _currentFov;
        private readonly float _fovSmoothSpeed = 10;

        private void OnValidate()
        {
            _weaponManager = GetComponent<WeaponManager>();
        }

        private void Start()
        {
            _inputHolder.PlayerActions.Aim.started += StartAim;
            _inputHolder.PlayerActions.Aim.canceled += EndAim;
            _currentFov = _hipFov = _camera.m_Lens.FieldOfView;

            _inputHolder.PlayerActions.Scroll.started += ChangeFromDirection;
            _inputHolder.PlayerActions.WeaponSlot1.started += ChangeToSlot1;
            _inputHolder.PlayerActions.WeaponSlot2.started += ChangeToSlot2;
            _inputHolder.PlayerActions.WeaponSlot3.started += ChangeToSlot3;
        }

        private void OnDestroy()
        {
            _inputHolder.PlayerActions.Aim.started -= StartAim;
            _inputHolder.PlayerActions.Jump.canceled -= EndAim;

            _inputHolder.PlayerActions.Scroll.started -= ChangeFromDirection;
            _inputHolder.PlayerActions.WeaponSlot1.started -= ChangeToSlot1;
            _inputHolder.PlayerActions.WeaponSlot2.started -= ChangeToSlot2;
            _inputHolder.PlayerActions.WeaponSlot3.started -= ChangeToSlot3;
        }


        private void Update()
        {
            RigCheck();

            AimUpdate();
        }

        private void RigCheck()
        {
            var screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
            var ray = Camera.main.ScreenPointToRay(screenCentre);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _aimMask, QueryTriggerInteraction.Ignore))
                _aimPosition.position = Vector3.Lerp(_aimPosition.position, hit.point, _aimSmoothSpeed * Time.deltaTime);
        }

        private void AimUpdate()
        {
            _camera.m_Lens.FieldOfView = Mathf.Lerp(_camera.m_Lens.FieldOfView, _currentFov, _fovSmoothSpeed * Time.deltaTime);

            var aimDirection = _inputHolder.PlayerActions.Look.ReadValue<Vector2>();

            var verticalDirection = _aimTarget.eulerAngles.x - aimDirection.y * .1f;
            var horizontalDirection = transform.eulerAngles.y + aimDirection.x * .1f;

            verticalDirection = verticalDirection switch
            {
                > 60 and < 180 => 60,
                > 180 and < 300 => 300,
                _ => verticalDirection
            };

            _aimTarget.eulerAngles = new Vector3(verticalDirection, _aimTarget.eulerAngles.y, 0);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, horizontalDirection, 0);
        }


        private void StartAim(InputAction.CallbackContext obj)
        {
            _currentFov = _adsFov;
        }

        private void EndAim(InputAction.CallbackContext obj)
        {
            _currentFov = _hipFov;
        }

        private void ChangeToSlot1(InputAction.CallbackContext obj)
        {
            _weaponManager.ChangeWeaponIndex(0);
        }

        private void ChangeToSlot2(InputAction.CallbackContext obj)
        {
            _weaponManager.ChangeWeaponIndex(1);
        }

        private void ChangeToSlot3(InputAction.CallbackContext obj)
        {
            _weaponManager.ChangeWeaponIndex(2);
        }

        private void ChangeFromDirection(InputAction.CallbackContext obj)
        {
            _weaponManager.ChangeWeaponIndexFromDirection(obj.ReadValue<float>());
        }

    }
}
