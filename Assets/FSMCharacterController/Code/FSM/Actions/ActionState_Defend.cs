namespace FSMCharacterController
{
    /// <summary>
    /// Action state for defending.
    /// </summary>
    public class ActionState_Defend : ActionState
    {
        public ActionState_Defend(FSMManager fsmManager)
        {
            _fsmManager = fsmManager;
            CharacterActionID = ActionID.Defend;
        }

        public override void Enter()
        {
            if (!_animationManager)
            {
                _animationManager = AnimationManager.Instance;
            }

            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_DEFENDING_FLAG, true);
        }

        
        public override void Execute()
        {
            ///Just a basic implementation of the state, so no fancy stuff to execute.
        }

        public override void Exit()
        {
            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_DEFENDING_FLAG, false);
        }
    }
}