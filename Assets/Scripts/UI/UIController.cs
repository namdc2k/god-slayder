using System;
using Core;
using Observer;
using UnityEngine;
using UnityEngine.UI;

namespace UI{
    public class UIController : MonoBehaviour, ISkill{
        [SerializeField] VariableJoystick joystick;
        [SerializeField] private Button attackBtn;
        [SerializeField] private Button dashBtn;
        [SerializeField] private Button specialAttackBtn;

        private void Start() {
            attackBtn.onClick.AddListener(AttackClickHandle);
            dashBtn.onClick.AddListener(DashClickHandle);
            specialAttackBtn.onClick.AddListener(SpecialAttackClickHandle);
        }

        private void Update() {
            GameEvent.OnChangeInputControl?.OnNext((joystick.Horizontal, joystick.Vertical));
        }

        public void AttackClickHandle() {
            
        }

        public void DashClickHandle() {
            
        }

        public void SpecialAttackClickHandle() {
            
        }
    }
}