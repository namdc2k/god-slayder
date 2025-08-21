using System;
using Observer;
using UnityEngine;

namespace UI{
    public class UIController : MonoBehaviour{
        [SerializeField] VariableJoystick joystick;

        private void Update() {
            GameEvent.OnChangeInputControl?.Invoke(joystick.Horizontal, joystick.Vertical);
        }
    }
}