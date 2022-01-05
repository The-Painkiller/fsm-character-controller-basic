using UnityEngine;

namespace FSMCharacterController
{
    public enum AttackTypes
    {
        LightAttack01 = 1,
        LightAttack02 = 2,
        HeavyAttack01 = 3
    }

    /// <summary>
    /// Player's or character's settings.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerControlSettings", menuName = "FSMCharacterController/New PlayerControlSettings", order = 1)]
    public class PlayerControlSettings : ScriptableObject
    {
        public float WalkingSpeed = 1f;
        public float RunningSpeed = 2f;
        public float CrouchWalkingSpeed = 0.5f;
        public float JumpForce = 300f;
        public float MouseSensitivity = 0.1f;
        public float SpeedTransitionFactor = 0.05f;
        public float OrientationTransitionFactor = 0.05f;
        public float JumpActivationDelayInSeconds = 0.5f;
        public int AttacksBeforeComboHeavyAttack = 2;
    }
}