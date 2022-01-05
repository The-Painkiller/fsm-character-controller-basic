namespace FSMCharacterController
{
    /// <summary>
    /// Movement state for crouching.
    /// </summary>
    public class MovementState_Crouch : MovementState
    {
        public MovementState_Crouch(FSMManager fsmManager)
        {
            _fsmManager = fsmManager;
            CharacterMovementID = MovementID.Crouch;
        }

        public override void Enter()
        {
            if (!_animationManager)
            {
                _animationManager = AnimationManager.Instance;
            }

            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_CROUCHING, true);

            ///No movement when you crouch.
            ///Of course this can be tweaked to have a crouched movement animation,
            ///and not keep the horizontal and vertical axes at zero.
            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_BLEND_HORIZONTAL, 0f);
            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_BLEND_VERTICAL, 0f);
        }

        public override void Execute()
        {
            _fsmManager.MovementManager.SetMovementDirection(_fsmManager.PlayerMovementDirection, 0f);
        }

        public override void Exit()
        {
            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_CROUCHING, false);
        }
    }
}