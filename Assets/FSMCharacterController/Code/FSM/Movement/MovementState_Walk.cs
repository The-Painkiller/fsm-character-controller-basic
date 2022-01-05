using UnityEngine;

namespace FSMCharacterController
{
    /// <summary>
    /// Movement state for walking.
    /// </summary>
    public class MovementState_Walk : MovementState
    {
        private float _playerSpeed = 0f;
        private float _currentSpeed = 0f;
        private float _speedTransitionTime = 0f;

        private Vector2 _moveDirection = Vector2.zero;

        private PlayerControlSettings _playerControlSettings;

        public MovementState_Walk(FSMManager fsmManager)
        {
            _fsmManager = fsmManager;
            _playerControlSettings = _fsmManager.PlayerControllerSettings;
            CharacterMovementID = MovementID.Walk;
        }

        public override void Enter()
        {
            if (!_animationManager)
            {
                _animationManager = AnimationManager.Instance;
            }

            _playerSpeed = _playerControlSettings.WalkingSpeed;
            _currentSpeed = _fsmManager.MovementManager.CurrentSpeed;
            _speedTransitionTime = 0f;
            
            AnimatePlayerMovement();
        }

        public override void Execute()
        {
            if (!AreEqual(_currentSpeed, _playerSpeed))
            {
                ///speed and inputs are Lerped to prevent choppy movements going from 0 to movement speed in 0.1 seconds :D
                _currentSpeed = Mathf.Lerp(_currentSpeed, _playerSpeed, _speedTransitionTime);
                _speedTransitionTime += _playerControlSettings.SpeedTransitionFactor;
            }
            else
            {
                _speedTransitionTime = 0f;
                _currentSpeed = _playerSpeed;
            }

            _moveDirection = _fsmManager.PlayerMovementDirection;
            _fsmManager.MovementManager.SetMovementDirection(_moveDirection, _currentSpeed);

            AnimatePlayerMovement();
        }

        public override void Exit()
        {
            ///nothing fancy happens here in this implementation.
        }

        private bool AreEqual(float oldValue, float newValue)
        {
            return Mathf.Abs(newValue - oldValue) < 0.01f;
        }

        private void AnimatePlayerMovement()
        {
            ///This entire patch takes care of how the character moves.
            ///If it's in Fighting mode, character's orientation doesn't change, and strafing is used.
            ///If it's not in Fighting mode, then the character's orientation is changed to left/right/back/front, and only vertical animation blending is used.
            ///This lets character animate its normal walk cycle no matter which input.

            if (_fsmManager.IsFighting)
            {
                _animationManager.SetParameter(AnimationManager.ANIM_PARAM_BLEND_HORIZONTAL, _fsmManager.PlayerMovementDirection.x * _currentSpeed);

                _animationManager.SetParameter(AnimationManager.ANIM_PARAM_BLEND_VERTICAL, _fsmManager.PlayerMovementDirection.y * _currentSpeed);
            }
            else
            {
                if (_fsmManager.PlayerMovementDirection.y == 0f && _fsmManager.PlayerMovementDirection.x != 0f)
                {
                    float xAxisValue = Mathf.Abs(_fsmManager.PlayerMovementDirection.x) > 0f ? 1f : 0f;
                    _animationManager.SetParameter(AnimationManager.ANIM_PARAM_BLEND_VERTICAL, xAxisValue * _currentSpeed);
                }
                else
                {
                    float yAxisValue = Mathf.Abs(_fsmManager.PlayerMovementDirection.y) > 0f ? 1f : 0f;
                    _animationManager.SetParameter(AnimationManager.ANIM_PARAM_BLEND_VERTICAL, yAxisValue * _currentSpeed);
                }
            }

            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_MOVE_SPEED, _currentSpeed);
        }
    }
}