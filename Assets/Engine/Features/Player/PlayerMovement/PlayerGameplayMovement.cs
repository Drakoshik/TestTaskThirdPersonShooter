using System.Collections;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TPS.Engine.Features.Player.PlayerMovement
{
    using Data;
    internal sealed class PlayerGameplayMovement : MonoBehaviour
    {
        [SerializeField] private PlayerSO _data = default!;
        [SerializeField] private PlayerInputHolder _inputHolder = default!;

        [SerializeField] public UnityEvent OnLand;

        [SerializeField] private float _gravity = -9.81f;

        [SerializeField] private float _groundYOffset = .6f;
        [SerializeField] private LayerMask _groundMask;

        [ShowInInspector, ReadOnly] private CharacterController _characterController;

        private Vector3 _velocity;
        private float _currentMovementSpeed;
        private Vector3 _spherePosition;
        [ShowInInspector, ReadOnly] private bool _isGrounded;

        private void OnValidate()
        {
            if(_characterController == null)
                _characterController = GetComponent<CharacterController>();
        }


        private void OnEnable()
        {
            _currentMovementSpeed = _data.MovementSpeed;
        }

        private void Start()
        {
            _inputHolder.PlayerActions.Jump.performed += Jump;
            _inputHolder.PlayerActions.Sprint.performed += SprintStarted;
            _inputHolder.PlayerActions.Sprint.canceled += SprintEnded;
            _isGrounded = IsGrounded();
        }

        private void OnDestroy()
        {
            _inputHolder.PlayerActions.Jump.performed -= Jump;
            _inputHolder.PlayerActions.Sprint.performed -= SprintStarted;
            _inputHolder.PlayerActions.Sprint.canceled -= SprintEnded;
        }

        private void Update()
        {
            Move();
            AddGravity();
        }

        private void Move()
        {
            var dir = transform.forward * _inputHolder.PlayerActions.Movement.ReadValue<Vector2>().y + transform.right * _inputHolder.PlayerActions.Movement.ReadValue<Vector2>().x;

            _characterController.Move(dir.normalized * (_currentMovementSpeed * Time.deltaTime));
        }

        private bool IsGrounded()
        {
            _spherePosition = new Vector3(transform.position.x, transform.position.y - _groundYOffset, transform.position.z);
            return Physics.CheckSphere(_spherePosition, _characterController.radius, _groundMask);
        }

        private void AddGravity()
        {
            if (!IsGrounded()) _velocity.y += _gravity * Time.deltaTime;
            else if (_velocity.y < 0) _velocity.y = -2;

            if(_isGrounded == false && IsGrounded())
            {
                OnLand.Invoke();
                _isGrounded = true;
            }

            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_spherePosition, _characterController.radius);
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            if (!IsGrounded()) return;

            StartCoroutine(WaitUnGrounded());
            _velocity.y = 0;
            _velocity.y += _data.JumpForce;
        }

        private IEnumerator WaitUnGrounded()
        {
            yield return new WaitForSeconds(.01f);

            _isGrounded = false;
        }
        private void SprintStarted(InputAction.CallbackContext obj)
        {
            _currentMovementSpeed = _data.SprintSpeed;
        }
        private void SprintEnded(InputAction.CallbackContext obj)
        {
            _currentMovementSpeed = _data.MovementSpeed;
        }
    }
}
