using UnityEngine;

namespace FSMCharacterController
{
    public enum MovementID
    {
        Idle,
        Walk,
        Run,
        Jump,
        Crouch
    }

    public enum ActionID
    {
        None,
        Attack,
        Defend
    }

    /// <summary>
    /// All states get controlled by this class.
    /// </summary>
    [RequireComponent(typeof(InputControlsManager), typeof(AnimationManager), typeof(CharacterMovementManager))]
    public class FSMManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerControlSettings _playerControlSettings = null;
        public PlayerControlSettings PlayerControllerSettings => _playerControlSettings;

        [SerializeField]
        private bool _isFighting = false;
        public bool IsFighting => _isFighting;

        private Vector2 _playerMovementDirection;
        public Vector2 PlayerMovementDirection => _playerMovementDirection;

        private InputControlsManager _inputManager = null;
        public InputControlsManager InputManager => _inputManager;

        private CharacterMovementManager _movementManager = null;
        public CharacterMovementManager MovementManager => _movementManager;

        private Transform _playerTransform;
        public Transform PlayerTransform => _playerTransform;

        public bool IsRunning { get; private set; } = false;
        public bool IsMoving { get; private set; } = false;
        public bool IsCrouching { get; private set; } = false;
        public bool IsJumping { get; private set; } = false;
        public bool IsAttacking { get; private set; } = false;
        public bool IsDefending { get; private set; } = false;

        private MovementState _stateIdle = null;
        private MovementState _stateWalk = null;
        private MovementState _stateRun = null;
        private MovementState _stateJump = null;
        private MovementState _stateCrouch = null;
        private MovementState _currentMovementState = null;

        private ActionState _actionNone = null;
        private ActionState _actionAttack = null;
        private ActionState _actionDefend = null;
        private ActionState _currentAction = null;

        private MovementID _currentMovementID;
        public MovementID CurrentMovementID => _currentMovementID;

        private ActionID _currentActionID;
        public ActionID CurrentActionID => _currentActionID;

        public static FSMManager Instance = null;

        private void Awake()
        {
            _playerTransform = transform;
            _inputManager = GetComponent<InputControlsManager>();
            _movementManager = GetComponent<CharacterMovementManager>();

            if (_inputManager == null || _movementManager == null)
            {
                gameObject.SetActive(false);
                enabled = false;
                return;
            }

            ///Initialize all states, movement and action.
            
            _stateIdle = new MovementState_Idle(this);
            _stateWalk = new MovementState_Walk(this);
            _stateRun = new MovementState_Run(this);
            _stateJump = new MovementState_Jump(this);
            _stateCrouch = new MovementState_Crouch(this);

            SetState(MovementID.Idle);

            _actionNone = new ActionState_None(this);
            _actionAttack = new ActionState_Attack(this);
            _actionDefend = new ActionState_Defend(this);

            SetAction(ActionID.None);

            AnimationEventManager.AttackComplete += OnAttackComplete;

            if (!Instance)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            ///Listening to all the inputs.

            _inputManager.Walk += OnWalk;
            _inputManager.Run += OnRun;
            _inputManager.Jump += OnJump;
            _inputManager.Crouch += OnCrouch;

            _inputManager.Attack += OnAttack;
            _inputManager.Defend += OnDefend;
        }

        private void OnDestroy()
        {
            _inputManager.Walk -= OnWalk;
            _inputManager.Run -= OnRun;
            _inputManager.Jump -= OnJump;
            _inputManager.Crouch -= OnCrouch;

            _inputManager.Attack -= OnAttack;
            _inputManager.Defend -= OnDefend;

            AnimationEventManager.AttackComplete -= OnAttackComplete;
        }

        private void OnDefend(object sender, bool defending)
        {
            IsDefending = defending;
            if (IsDefending)
            {
                SetAction(ActionID.Defend);
            }
            else
            {
                SetAction(ActionID.None);
            }
        }

        private void OnAttack(object sender, bool attacking)
        {
            IsAttacking = attacking;
            if (IsAttacking)
            {
                SetAction(ActionID.Attack);
            }
            else if (IsDefending)
            {
                SetAction(ActionID.Defend);
            }
        }

        private void OnCrouch(object sender, bool crouching)
        {
            if (IsJumping)
                return;

            IsCrouching = crouching;

            if (IsCrouching && !IsMoving)
            {
                SetState(MovementID.Crouch);
            }
            else if (!IsCrouching && !IsMoving)
            {
                SetState(MovementID.Idle);
                return;
            }
        }

        private void OnJump(object sender, bool jumping)
        {
            IsJumping = jumping && !IsCrouching;

            if (IsJumping)
                SetState(MovementID.Jump);
        }

        private void OnRun(object sender, bool running)
        {
            if (IsJumping)
                return;

            IsRunning = running;

            if (IsRunning && IsMoving)
            {
                SetState(MovementID.Run);
            }
            else if (!IsRunning && IsMoving)
            {
                SetState(MovementID.Walk);
            }
        }

        private void OnWalk(object sender, Vector2 direction)
        {
            _playerMovementDirection = direction;
            
            if (IsJumping)
                return;

            if (_playerMovementDirection == Vector2.zero)
            {
                if (IsCrouching)
                {
                    SetState(MovementID.Crouch);
                }
                else
                {
                    SetState(MovementID.Idle);
                }
                IsMoving = false;
            }
            else
            {
                IsMoving = true;

                if (IsRunning)
                {
                    SetState(MovementID.Run);
                }
                else
                {
                    SetState(MovementID.Walk);
                }

            }
        }

        private void OnAttackComplete()
        {
            if (IsDefending)
                SetAction(ActionID.Defend);
            else
                SetAction(ActionID.None);
        }

        private void FixedUpdate()
        {
            ///All movement state executes get called here.
            
            if (_currentMovementState != null)
            {
                _currentMovementState.Execute();
            }
        }

        private void Update()
        {
            ///All action state executes get called here.

            if (_currentAction != null && _currentAction != _actionNone)
            {
                _currentAction.Execute();
            }
        }

        /// <summary>
        /// This is called to transition to next passed state after exiting current state.
        /// Mostly controlled by this class itself, states are changed as per the inputs passed.
        /// </summary>
        /// <param name="id">Action State ID</param>
        public void SetAction(ActionID id)
        {
            if (_currentAction != null)
            {
                if (_currentAction.CharacterActionID == id)
                {
                    return;
                }

                _currentAction.Exit();
            }

            switch (id)
            {
                case ActionID.None:
                    _currentAction = _actionNone;
                    break;

                case ActionID.Attack:
                    _currentAction = _actionAttack;
                    break;

                case ActionID.Defend:
                    _currentAction = _actionDefend;
                    break;

                default:
                    _currentAction = _actionNone;
                    break;
            }

            _currentActionID = id;
            _currentAction.Enter();
        }

        /// <summary>
        /// This is called to transition to next passed state after exiting current state.
        /// /// Mostly controlled by this class itself, states are changed as per the inputs passed.
        /// </summary>
        /// <param name="id">Movement State ID</param>
        public void SetState(MovementID id)
        {
            if (_currentMovementState != null)
            {
                if (_currentMovementState.CharacterMovementID == id)
                {
                    return;
                }

                _currentMovementState.Exit();
            }

            switch (id)
            {
                case MovementID.Idle:
                    _currentMovementState = _stateIdle;
                    break;

                case MovementID.Walk:
                    _currentMovementState = _stateWalk;
                    break;

                case MovementID.Run:
                    _currentMovementState = _stateRun;
                    break;

                case MovementID.Jump:
                    _currentMovementState = _stateJump;;
                    break;

                case MovementID.Crouch:
                    _currentMovementState = _stateCrouch;
                    break;

                default:
                    _currentMovementState = _stateIdle;
                    break;
            }

            _currentMovementID = id;
            _currentMovementState.Enter();
        }

        /// <summary>
        /// If some other class has to request a state exit, for example Jump State,
        /// This can be called, and it will make sure to transition to next state properly.
        /// </summary>
        /// <param name="requestingState"></param>
        public void RequestStateExit(MovementID requestingState)
        {
            if (requestingState == MovementID.Jump)
            {
                IsJumping = false;
                if (_playerMovementDirection == Vector2.zero)
                {
                    if (IsCrouching)
                    {
                        SetState(MovementID.Crouch);
                    }
                    else
                    {
                        SetState(MovementID.Idle);
                    }
                    IsMoving = false;
                }
                else
                {
                    IsMoving = true;

                    if (IsRunning)
                    {
                        SetState(MovementID.Run);
                    }
                    else
                    {
                        SetState(MovementID.Walk);
                    }

                }
            }
        }

        public void SetFightingMode(bool active)
        {
            _isFighting = active;
        }
    }
}