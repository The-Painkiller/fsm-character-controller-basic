namespace FSMCharacterController
{
    /// <summary>
    /// Action state to get out of all actions.
    /// </summary>
    public class ActionState_None : ActionState
    {
        public ActionState_None(FSMManager fsmManager)
        {
            _fsmManager = fsmManager;
            CharacterActionID = ActionID.None;
        }

        public override void Enter()
        {
            if (!_animationManager)
            {
                _animationManager = AnimationManager.Instance;
            }

            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_ATTACKING_FLAG, false);
        }

        ///Nothing fancy happens here in this implementation.
        ///May be Execute can be used to play random idle animations 
        ///based on time and movement states...
        
        public override void Execute()
        {
            
        }

        public override void Exit()
        {
        }
    }
}