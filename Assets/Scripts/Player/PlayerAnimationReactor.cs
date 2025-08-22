using System;
using Observer;
using UnityEngine;
using R3;

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

        private void Start() {
            GameEvent.OnChangeAnimationClip.Subscribe(_ => OnChangeAnimationClip(_.Item1, _.Item2)).AddTo(this);
        }

        private void OnChangeAnimationClip(float speed, PlayerAnimationClip clip) {
            if (clip != PlayerAnimationClip.Running) {
                var id = Animator.StringToHash(clip.ToString());
                animator.SetTrigger(id);
                return;
            }

            animator.SetFloat("Speed", speed);
        }

        private void EndAction() {
            Debug.Log("Ending action");
        }
    }
}