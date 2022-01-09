using System;
using UnityEngine;
using UnityEngine.UI;

namespace FSMCharacterController
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Toggle _fightingToggle;

        private FSMManager _fsmManager;
        // Start is called before the first frame update
        void Start()
        {
            _fsmManager = FSMManager.Instance;

            _fightingToggle.onValueChanged.AddListener(OnFightModeValueChanged);
        }

        private void OnFightModeValueChanged(bool active)
        {
            _fsmManager.SetFightingMode(active);
        }
    }
}