namespace FSMCharacterController
{
    /// <summary>
    /// Base class for actions.
    /// </summary>
    public abstract class ActionState
    {
        protected FSMManager _fsmManager;
        protected AnimationManager _animationManager;

        public ActionID CharacterActionID { get; protected set; }

        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}