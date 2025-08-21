using System;
using Observer;
using UnityEngine;

namespace Player{
    public enum PlayerAnimationClip{
        Idle,
        Running,
    }

    public class PlayerAnimationReactor : MonoBehaviour{
        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void OnEnable() {
            GameEvent.OnChangeAnimationClip += OnChangeAnimationClip;
        }

        private void OnChangeAnimationClip(float speed, PlayerAnimationClip clip) {
            if (clip != PlayerAnimationClip.Running) {
                var id = Animator.StringToHash(clip.ToString());
                animator.SetTrigger(id);
                return;
            }
            animator.SetFloat("Speed", speed);
        }
    }
}