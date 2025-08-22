using System;
using Core;
using Observer;
using UnityEngine;
using R3;

namespace Player{
    public class PlayerMovement : MonoBehaviour, IMoveable{
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float rotateSpeed = 5f;
        private Rigidbody _rigidbody;
        Vector3 _inputVector = Vector3.zero;
        float _horizontal;
        float _vertical;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start() {
            GameEvent.OnChangeInputControl.Subscribe(_ => SetInputControl(_.Item1, _.Item2)).AddTo(this);
        }

        private void Update() {
#if UNITY_EDITOR
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
#endif
        }

        private void FixedUpdate() {
            _inputVector.x = _horizontal;
            _inputVector.z = _vertical;
            Move(_inputVector, Time.fixedDeltaTime);
            Rotate(_inputVector, Time.fixedDeltaTime);
        }


        private void SetInputControl(float horizontal, float vertical) {
#if !UNITY_EDITOR
            _horizontal = horizontal;
            _vertical = vertical;
#endif
        }

        public void Move(Vector3 dir, float duration) {
            GameEvent.OnChangeAnimationClip?.OnNext((dir.magnitude, PlayerAnimationClip.Running));
            Vector3 nextVec = dir.normalized * moveSpeed * duration;
            _rigidbody.MovePosition(_rigidbody.position + nextVec);
        }

        public void Rotate(Vector3 dir, float duration) {
            if (dir.magnitude >= 0.1f) {
                Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * duration);
            }
        }

        public void Dead() {
        }
    }
}