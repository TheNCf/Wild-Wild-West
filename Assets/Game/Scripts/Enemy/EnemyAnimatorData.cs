using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyAnimatorData
{
    public static class Params
    {
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
        public static readonly int Hit = Animator.StringToHash(nameof(Hit));
        public static readonly int IsDead = Animator.StringToHash(nameof(IsDead));
        public static readonly int IsShooting = Animator.StringToHash(nameof(IsShooting));
    }
}
