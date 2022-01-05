using UnityEngine;

namespace FSMCharacterController
{
    /// <summary>
    /// Movement state for jumping.
    /// </summary>
    public class MovementState_Jump : MovementState
    {
        private bool _isGrounded = false;
        private Vector3 _jumpForceVector = Vector3.zero;
        private float _timer = 0f;

        private PlayerControlSettings _playerControlSettings;

        public MovementState_Jump(FSMManager fsmManager)
        {
            _fsmManager = fsmManager;
            _playerControlSettings = _fsmManager.PlayerControllerSettings;
            CharacterMovementID = MovementID.Jump;
        }

        public override void Enter()
        {
            if (!_animationManager)
            {
                _animationManager = AnimationManager.Instance;
            }

            ///On entering, an upward force is applied here
            ///and a Jump Begin animation is played in loop.

            _animationManager.SetTrigger(AnimationManager.ANIM_PARAM_JUMPING);

            _jumpForceVector = Vector3.up * _playerControlSettings.JumpForce;
            _jumpForceVector.x *= (_fsmManager.MovementManager.CurrentSpeed / 2.0f);
            _fsmManager.MovementManager.AddJumpForce(_jumpForceVector);
            _timer = 0f;
        }

        public override void Execute()
        {
            ///This timer is used because it takes a few frames for the
            ///raycasting to return false for the ground.
            ///So you start raycasting after a short period of time.
            _timer += Time.fixedDeltaTime;
            if (_timer < _playerControlSettings.JumpActivationDelayInSeconds)
                return;

            _isGrounded = _fsmManager.MovementManager.IsInAir();

            ///In case of jumping, since the state itself checks whether the player is
            ///in air or ground, the state requests for a state change from FSM manager, 
            ///once the player is grounded back.
            
            if (_isGrounded)
            {
                _fsmManager.RequestStateExit(CharacterMovementID);
            }
        }

        public override void Exit()
        {
            ///Plays a Jump End animation.
            _animationManager.SetTrigger(AnimationManager.ANIM_PARAM_LANDING);
        }
    }
}