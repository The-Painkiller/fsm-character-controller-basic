using UnityEngine;

namespace FSMCharacterController
{
    /// <summary>
    /// Attack action state. Controls light attacking actions, and combo.
    /// </summary>
    public class ActionState_Attack : ActionState
    {
        private int _currentAttackMoveID = 1;
        private int _attackTypesRange = (int)AttackTypes.HeavyAttack01;
        private bool _isAttacking = false;
        private int _attacksBeforeHeavyComboAttack = 0;
        private static int _consecutiveAttackCalls = 0;
        private static float _currentAttackTime = 0f;
        private const float MAX_TIME_FOR_HEAVY_ATTACK = 1f;

        public ActionState_Attack(FSMManager fsmManager)
        {
            _fsmManager = fsmManager;
            CharacterActionID = ActionID.Attack;
            _attacksBeforeHeavyComboAttack = _fsmManager.PlayerControllerSettings.AttacksBeforeComboHeavyAttack;
        }

        public override void Enter()
        {
            if (!_animationManager)
            {
                _animationManager = AnimationManager.Instance;
            }

            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_ATTACKING_FLAG, true);

            ///If consecutive light attacks were called within a given time, 
            ///then keep adding them up.
            if (Time.realtimeSinceStartup - _currentAttackTime < MAX_TIME_FOR_HEAVY_ATTACK)
            {
                _consecutiveAttackCalls++;
            }
            else
            {
                _consecutiveAttackCalls = 0;
            }
            _currentAttackTime = Time.realtimeSinceStartup;
        }

        public override void Execute()
        {
            if (_isAttacking)
            {
                return;
            }

            ///If consecutive light attacks add up to combo attack range,
            ///then play combo attack.
            if (_consecutiveAttackCalls < _attacksBeforeHeavyComboAttack)
            {
                _currentAttackMoveID = Random.Range(1, _attackTypesRange);
            }
            else
            {
                _currentAttackMoveID = (int)AttackTypes.HeavyAttack01;
                _consecutiveAttackCalls = 0;
            }

            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_ATTACK_MOVE, _currentAttackMoveID);
            _isAttacking = true;
        }

        public override void Exit()
        {
            _isAttacking = false;
            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_ATTACKING_FLAG, false);
            _animationManager.SetParameter(AnimationManager.ANIM_PARAM_ATTACK_MOVE, 0);
        }
    }
}