using UnityEngine;

namespace FSMCharacterController
{
    /// <summary>
    /// Animator gets controlled directly by this class.
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public static AnimationManager Instance = null;

        public const string ANIM_PARAM_BLEND_HORIZONTAL = "BlendHorizontal";
        public const string ANIM_PARAM_BLEND_VERTICAL = "BlendVertical";
        public const string ANIM_PARAM_MOVE_SPEED = "MoveSpeed";
        public const string ANIM_PARAM_JUMPING = "Jumping";
        public const string ANIM_PARAM_LANDING = "Landing";
        public const string ANIM_PARAM_CROUCHING = "Crouching";
        public const string ANIM_PARAM_ATTACK_MOVE = "AttackMove";
        public const string ANIM_PARAM_ATTACKING_FLAG = "Attacking";
        public const string ANIM_PARAM_DEFENDING_FLAG = "Defending";

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }

        public void SetParameter(string parameter, bool value)
        {
            _animator.SetBool(parameter, value);
        }

        public void SetParameter(string parameter, int value)
        {
            _animator.SetInteger(parameter, value);
        }

        public void SetParameter(string parameter, float value)
        {
            _animator.SetFloat(parameter, value);
        }

        public void SetParameter(string triggerParameter)
        {
            _animator.SetTrigger(triggerParameter);
        }

        public void SetTrigger(string trigger)
        {
            _animator.SetTrigger(trigger);
        }

        public void ResetAttackMove()
        {
            SetParameter("AttackMove", 0);
            SetParameter("Attacking", false);
        }

        public void AttackCompleted()
        {
            Debug.Log("Attack Finish");
        }
    }
}