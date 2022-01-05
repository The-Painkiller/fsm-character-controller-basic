using UnityEngine;

namespace FSMCharacterController
{
    /// <summary>
    /// Movement state for being idle.
    /// </summary>
    public class MovementState_Idle : MovementState
    {
        private float _playerSpeed = 0f;
        private float _currentSpeed = 0f;
        private float _speedTransitionTime = 0f;

        private PlayerControlSettings _playerControlSettings;

        public MovementState_Idle(FSMManager fsmManager)
        {
            _fsmManager = fsmManager;
            _playerControlSettings = _fsmManager.PlayerControllerSettings;
            CharacterMovementID = MovementID.Idle;
        }

        ///Pretty simple implementation here. Exit override does nothing in this case.
        ///This could be used to play animations, or movement sounds, or whatever.

        public override void Enter()
        {
            if (!_animationManager)
            {
                _animationManager = AnimationManager.Instance;
            }

            _speedTransitionTime = 0f;
            _currentSpeed = _fsmManager.MovementManager.CurrentSpeed;
            _fsmManager.MovementManager.SetMovementDirection(Vector2.zero, 0f);
        }

        public override void Execute()
        {
            if (!AreEqual(_currentSpeed, _playerSpeed))
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed, _playerSpeed, _speedTransitionTime);
                _speedTransitionTime += _playerControlSettings.SpeedTransitionFactor;
            }
            else
            {
                _currentSpeed = 0f;
                _speedTransitionTime = 0f;
            }

            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_BLEND_VERTICAL, _currentSpeed);
            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_BLEND_HORIZONTAL, _currentSpeed);
            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_MOVE_SPEED, _currentSpeed);

        }

        public override void Exit()
        {
        }

        private bool AreEqual(float oldValue, float newValue)
        {
            return Mathf.Abs(newValue - oldValue) < 0.01f;
        }
    }
}