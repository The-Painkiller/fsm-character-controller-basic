using System;
using UnityEngine;

namespace FSMCharacterController
{
    /// <summary>
    /// The generated Input Controls class gets listened to here,
    /// and events are fired in return for other classes to know what input was triggered.
    /// </summary>
    public class InputControlsManager : MonoBehaviour
    {
        private InputControls _inputControls;

        public event EventHandler<Vector2> Walk;
        public event EventHandler<bool> Run;
        public event EventHandler<bool> Crouch;
        public event EventHandler<bool> Jump;

        public event EventHandler<bool> Attack;
        public event EventHandler<bool> Defend;

        public event EventHandler<Vector2> Look;

        private Vector2 _lookDirection = Vector2.zero;

        private void Awake()
        {
            _inputControls = new InputControls();

            _inputControls.InputActions.Movement.performed += (moveVector) =>
            {
                Vector2 movementVector = moveVector.ReadValue<Vector2>();

                Walk?.Invoke(this, movementVector);
            };

            _inputControls.InputActions.Run.started += (runToggle) =>
            {
                Run?.Invoke(this, true);
            };

            _inputControls.InputActions.Run.canceled += (runToggle) =>
            {
                Run?.Invoke(this, false);
            };

            _inputControls.InputActions.Crouch.started += (crouchToggle) =>
            {
                Crouch?.Invoke(this, true);
            };

            _inputControls.InputActions.Crouch.canceled += (crouchToggle) =>
            {
                Crouch?.Invoke(this, false);
            };

            _inputControls.InputActions.Jump.started += (jumpToggle) =>
             {
                 Jump?.Invoke(this, true);
             };

            _inputControls.InputActions.Look.performed += (lookVector) =>
            {
                _lookDirection = lookVector.ReadValue<Vector2>();
                Look?.Invoke(this, _lookDirection);
            };

            _inputControls.InputActions.Attack.started += (callback) =>
            {
                Attack?.Invoke(this, true);
            };

            _inputControls.InputActions.Attack.canceled += (callback) =>
            {
                Attack?.Invoke(this, false);
            };

            _inputControls.InputActions.Defend.started += (callback) =>
            {
                Defend?.Invoke(this, true);
            };

            _inputControls.InputActions.Defend.canceled += (callback) =>
            {
                Defend?.Invoke(this, false);
            };
        }

        private void OnEnable()
        {
            _inputControls.Enable();
        }

        private void OnDisable()
        {
            _inputControls.Disable();
        }
    }
}