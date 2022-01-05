using UnityEngine;

namespace FSMCharacterController
{

    /// <summary>
    /// Camera movements are controlled in this class.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// This transform acts as an X Axis gimbal for the camera rotation.
        /// </summary>
        [SerializeField]
        private Transform _xAxisGimbal;
        
        private FSMManager _fsmManager;
        private PlayerControlSettings _playerControlSettings;

        private Camera _mainCamera;
        private Vector2 _cameraRotation = Vector2.zero;

        private Vector2 _currentLookDirection = Vector2.zero;
        public Vector2 CurrentLookDirection => _currentLookDirection;

        private void Awake()
        {
            _mainCamera = transform.GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            _fsmManager = FSMManager.Instance;
            _fsmManager.InputManager.Look += OnLook;
            if (!_fsmManager)
            {
                gameObject.SetActive(false);
                enabled = false;
                return;
            }

            _playerControlSettings = _fsmManager.PlayerControllerSettings;
        }

        private void OnLook(object sender, Vector2 lookDir)
        {
            _currentLookDirection = lookDir * _playerControlSettings.MouseSensitivity;

            _cameraRotation.x += _currentLookDirection.x;
            _cameraRotation.y += _currentLookDirection.y;

            ///Y axis rotation of the camera is clamped so that it doesn't roll weirdly around the character.
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -50, 50);
        }

        private void FixedUpdate()
        {
            if (!_mainCamera || !_fsmManager || !_xAxisGimbal)
                return;
            
            transform.eulerAngles = new Vector3(0, _cameraRotation.x, 0);
            _xAxisGimbal.localEulerAngles = new Vector3(-_cameraRotation.y, 0, 0);
        }
    }
}