using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
   private Animator _animator;

   private static class _keysAnimation
   {
      public const string Die = "isDied";
      public const string Idle = "isIdle";
      public const string Run = "isRunning";
      public const string Attack = "isAttacking";
   }

   public void Init(Animator animator)
   {
      _animator = animator;
   }

   public void IsDied()
   {
      _animator.SetTrigger(_keysAnimation.Die);
   }

   public void IsIdle()
   {
      _animator.SetBool(_keysAnimation.Run, false);
      _animator.SetBool(_keysAnimation.Attack, false);
   }

   public void IsAttacking()
   {
      _animator.SetBool(_keysAnimation.Attack, true);
      Debug.Log("12345566");
   }
   public void IsRunning()
   {
      _animator.SetBool(_keysAnimation.Run, true);
     // _animator.SetBool(_keysAnimation.Attack, false);
   }
}
