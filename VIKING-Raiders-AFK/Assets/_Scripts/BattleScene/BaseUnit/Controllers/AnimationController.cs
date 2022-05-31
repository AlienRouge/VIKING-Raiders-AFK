using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class _keysAnimation
{
   public static readonly string Die = "isDied";
   public static readonly string Idle = "isIdle";
   public static readonly string  Run = "isRunning";
   public static readonly string Attack = "isAttacking";
}

public class AnimationController : MonoBehaviour
{
   private Animator _animator;

   public void Init(Animator animator)
   {
      _animator = animator;
   }

   public void IsDied()
   {
      if (_animator != null) {
         _animator.SetTrigger(_keysAnimation.Die);
      }
   }

   public void IsIdle()
   {
      if (_animator != null)
      {
         _animator.SetBool(_keysAnimation.Run, false);
         _animator.SetBool(_keysAnimation.Attack, false);
      }
   }

   public void IsAttacking()
   {
      if (_animator != null)
      {
         _animator.SetBool(_keysAnimation.Attack, true);
         Debug.Log("12345566");
      }
   }
   public void IsRunning()
   {
      if (_animator != null)
      {
         _animator.SetBool(_keysAnimation.Run, true);
         // _animator.SetBool(_keysAnimation.Attack, false);
      }
   }
}
