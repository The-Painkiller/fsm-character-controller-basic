using System;
using UnityEngine;

namespace FSMCharacterController
{
    /// <summary>
    /// Simple class that is attached to the character itself.
    /// Catches Animation Events latched on attack animations,
    /// and fires an action in return.
    /// </summary>
    public class AnimationEventManager : MonoBehaviour
    {
        public static Action AttackComplete;
        public void AttackCompleted()
        {
            AttackComplete?.Invoke();
        }
    }
}