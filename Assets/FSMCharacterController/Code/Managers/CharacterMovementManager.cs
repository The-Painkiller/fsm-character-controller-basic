using UnityEngine;

namespace FSMCharacterController
{
    /// <summary>
    /// Character's actual movement or transform gets controlled from this task.
    /// </summary>
    public class CharacterMovementManager : MonoBehaviour
    {
        [SerializeField]
        private CameraController _cameraController;

        private Transform _transform = null;
        private Rigidbody _rigidBody = null;
        private FSMManager _fsmManager = null;
        private Collider _collider = null;
        private PlayerControlSettings _playerControlSettings;

        private Vector2 _movementDirection = Vector2.zero;        

        private Vector3 _newOrientation = Vector3.zero;
        private Vector3 _characterOrientation = Vector3.zero;

        private float _currentSpeed = 0f;
        public float CurrentSpeed => _currentSpeed;

        private float _orientationTransitionTime = 0f;

        private void Awake()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _fsmManager = GetComponent<FSMManager>();
            if (_fsmManager == null || _collider == null ||
                _rigidBody == null || _cameraController == null)
            {
                gameObject.SetActive(false);
                enabled = false;
            }

            _playerControlSettings = _fsmManager.PlayerControllerSettings;
            _characterOrientation = _transform.forward;
        }

        private void FixedUpdate()
        {
            _transform.position += _cameraController.transform.right * _movementDirection.x * Time.fixedDeltaTime * _currentSpeed
                 + _cameraController.transform.forward * _movementDirection.y * Time.fixedDeltaTime * _currentSpeed;

            ///Character orientation is restricted if the character is fighting.
            ///In this case strafing is implemented in Walk and Run states,
            ///and only the camera direction controls the character direction.
            if (!_fsmManager.IsFighting)
            {
                TransitionOrientation();
            }
            else
            {
                _transform.forward = _cameraController.transform.forward;
            }
        }

        /// <summary>
        /// Takes care of character's orientation when not fighting.
        /// If the input is on X Axis (A/D), then the character is rotated in that direction 
        /// corresponding to the camera's orientation, and moved accordingly.
        /// </summary>
        private void TransitionOrientation()
        {
            if (_movementDirection.x == 0f && _movementDirection.y != 0f)
            {
                _newOrientation = _cameraController.transform.forward * _movementDirection.y;
            }
            else if (_movementDirection.x != 0f && _movementDirection.y == 0f)
            {
                _newOrientation = _cameraController.transform.right * _movementDirection.x;
            }
            else if (_movementDirection.x != 0f && _movementDirection.y != 0f)
            {
                _newOrientation = _cameraController.transform.right * _movementDirection.x + _cameraController.transform.forward * _movementDirection.y;
            }


            if (AreEqual(_characterOrientation, _newOrientation))
            {
                _orientationTransitionTime = 0f;
                return;
            }

            ///Orientation is Lerped here.
            _characterOrientation = Vector3.Lerp(_characterOrientation, _newOrientation, _orientationTransitionTime);
            _transform.forward = _characterOrientation;
            _orientationTransitionTime += _playerControlSettings.OrientationTransitionFactor;
        }

        private bool AreEqual(Vector3 oldOrientation, Vector3 newOrientation)
        {
            return Vector3.Distance(oldOrientation, newOrientation) < 0.05f;
        }

        /// <summary>
        /// Set the movement direction as per Input passed, and the current speed.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="speed">Run or Walk speed gets passed generally.</param>
        public void SetMovementDirection(Vector2 direction, float speed)
        {
            _movementDirection = direction;
            _currentSpeed = speed;
        }

        /// <summary>
        /// Adds jump force in passed direction...which is supposed to be upwards.
        /// </summary>
        /// <param name="force"></param>
        public void AddJumpForce(Vector3 force)
        {
            _rigidBody.AddForce(force, ForceMode.Impulse);
        }

        /// <summary>
        /// Shoots raycast downwards.
        /// If it's false, then character is in Air, otherwise it's on the ground.
        /// </summary>
        /// <returns></returns>
        public bool IsInAir()
        {
            RaycastHit info = new RaycastHit();
            
            return (Physics.Raycast(_collider.bounds.center, Vector3.down, out info, _collider.bounds.extents.y + 0.1f));
        }
    }
}