using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterAnimatorData
{
    public static class Params
    {
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
        public static readonly int NoInput = Animator.StringToHash(nameof(NoInput));
        public static readonly int InputX = Animator.StringToHash(nameof(InputX));
        public static readonly int InputY = Animator.StringToHash(nameof(InputY));
        public static readonly int IsAiming = Animator.StringToHash(nameof(IsAiming));
    }
}
