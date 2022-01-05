namespace FSMCharacterController
{
    /// <summary>
    /// Base movement state.
    /// </summary>
    public abstract class MovementState
    {
        protected FSMManager _fsmManager;
        protected AnimationManager _animationManager;

        public MovementID CharacterMovementID { get; protected set; }

        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}