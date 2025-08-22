using System;
using Player;
using R3;
using UnityEngine;

namespace Observer{
    public class GameEvent : MonoBehaviour{
        private static GameEvent _instance;

#region private

        private Subject<(float, float)> onChangeInputControl;
        private Subject<(float, PlayerAnimationClip)> onChangeAnimationClip;

#endregion

#region Event

        public static Subject<(float, float)> OnChangeInputControl => _instance.onChangeInputControl;
        public static Subject<(float, PlayerAnimationClip)> OnChangeAnimationClip => _instance.onChangeAnimationClip;

#endregion

        private void Awake() {
            _instance = this;
            onChangeInputControl = new();
            onChangeAnimationClip = new();
            onChangeInputControl.AddTo(this);
            onChangeAnimationClip.AddTo(this);
        }
    }
}